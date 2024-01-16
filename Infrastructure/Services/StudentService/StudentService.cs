using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceScopeFactory _serviceScope;
    public StudentService(ApplicationDbContext context, IServiceScopeFactory serviceScope)
    {
        _context = context;
        _serviceScope = serviceScope;
    }

    public async Task<Response<List<GetStudentDto>>> GetStudentsAsync(StudentFilter filter)
    {
        try
        {
            var students = _context.Students.AsNoTracking().AsQueryable();

            if (filter.Name != null)
            {
                students = students.Where(x => x.FirstName.ToLower().Trim().Contains(filter.Name.ToLower().Trim()) ||
                                               x.LastName.ToLower().Trim().Contains(filter.Name.ToLower().Trim()));
            }

            var totalRecord = students.Count();
            var mapped = await students.Select(s => new GetStudentDto()
            {
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                Address = s.Address,
                Gender = s.Gender.ToString(),
                Id = s.Id,
                Status = s.Status.ToString(),
                BirthDate = s.BirthDate,
                PhoneNumber = s.PhoneNumber,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            return new PagedResponse<List<GetStudentDto>>(mapped, HttpStatusCode.OK, "Ok", totalRecord,
                filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new Response<List<GetStudentDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetStudentDto>> GetStudentsByIdAsync(int id)
    {
        try
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (student == null) return new Response<GetStudentDto>(HttpStatusCode.NotFound, "Student not found");
            var map = new GetStudentDto()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                BirthDate = student.BirthDate,
                Address = student.Address,
                Gender = student.Gender.ToString(),
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                Id = student.Id,
                Status = student.Status.ToString(),
                Status1 = student.Status,
                Gender1 = student.Gender
            };
            return new Response<GetStudentDto>(HttpStatusCode.OK, "Student found", map);
        }
        catch (Exception e)
        {
            return new Response<GetStudentDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> AddStudentAsync(AddStudentDto student)
    {
        try
        {
            var user = await _context.Students.FirstOrDefaultAsync(x => x.PhoneNumber == student.PhoneNumber);
            if (user != null) return new Response<string>(HttpStatusCode.BadRequest, "Student already exists");
            var map = new Student()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                CreateDate = DateTime.UtcNow,
                PhoneNumber = student.PhoneNumber,
                Address = student.Address,
                Gender = student.Gender,
                BirthDate = student.BirthDate,
                Email = student.Email,
                Status = student.Status,
            };

            await _context.Students.AddAsync(map);
            await _context.SaveChangesAsync();

            _ = Task.Run(async () =>
            {
               await using var scope = _serviceScope.CreateAsyncScope();
              var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
               
                Random random = new Random();
                var passowrd = random.Next(10).ToString();

                var newUser = new User
                {
                    HashPassword = passowrd,
                    CreateDate = DateTime.UtcNow,
                    Email = map.Email,
                    FirstName = map.FirstName,
                    LastName = map.LastName,
                    Id = map.Id,
                    Phone = map.PhoneNumber,
                    UpdateDate = map.UpdateDate,
                    ImageName = string.Empty,
                    IsBlocked = false,
                    UserName = map.PhoneNumber,
                    UserType = UserType.Student
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleName == DefaultRole.Student.ToString());
                if (role != null)
                {
                    var newUserRole = new UserRole
                    {
                        UserId = newUser.Id,
                        RoleId = role.Id
                    };

                    await _context.UserRoles.AddAsync(newUserRole);
                    await _context.SaveChangesAsync();
                }
            });
            
            return new Response<string>(HttpStatusCode.OK, "Student added  successfully");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateStudentAsync(UpdateStudentDto student)
    {
        try
        {
            var request = await _context.Students.FirstOrDefaultAsync(x => x.Id == student.Id);
            if (request == null) return new Response<string>(HttpStatusCode.NotFound, "Student not found");

            request.Address = student.Address;
            request.BirthDate = student.BirthDate;
            request.FirstName = student.FirstName;
            request.LastName = student.LastName;
            request.Email = student.Email;
            request.Gender = student.Gender;
            request.PhoneNumber = student.PhoneNumber;
            request.Status = student.Status;
            request.UpdateDate = DateTime.UtcNow;
            student.Id = student.Id;

            await _context.SaveChangesAsync();

            return new Response<string>(HttpStatusCode.OK, "Student updated successfully");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteStudentAsync(int id)
    {
        try
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (student == null) return new Response<bool>(HttpStatusCode.NotFound, "Student not found");
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return new Response<bool>(HttpStatusCode.OK, "Student deleted successfully");
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}