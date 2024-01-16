namespace Infrastructure
{
    public class FacultyService : IFacultyService
    {
        private readonly ApplicationDbContext _dbContext;
        public FacultyService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Response<AddFacultyDto>> AddFaculty(AddFacultyDto model, CancellationToken token = default)
        {
            try
            {
                var faculty = await _dbContext.Faculties.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower());

                if (faculty != null) return new Response<AddFacultyDto>(HttpStatusCode.Found, "Faculty already exists !");

                var newFaculty = new Faculty
                {
                    Name = model.Name,
                    Description = model.Description,
                    Status = model.Status
                };

                await _dbContext.Faculties.AddAsync(newFaculty, token);
                var result = await _dbContext.SaveChangesAsync(token);

                return result > 0
                    ? new Response<AddFacultyDto>(HttpStatusCode.OK, "Data successfully added !")
                    : new Response<AddFacultyDto>(HttpStatusCode.InternalServerError, "Data not added !");
            }
            catch (Exception ex)
            {
                return new Response<AddFacultyDto>(HttpStatusCode.InternalServerError,ex.Message);
            }
        }

        public async Task<Response<string>> DeleteFacultyById(int facultyId, CancellationToken token = default)
        {
            try
            {
                var faculty = await _dbContext.Faculties.FirstOrDefaultAsync(x => x.Id == facultyId, token);
                if (faculty == null) return new Response<string>(HttpStatusCode.NotFound, "Faculty not found !");

                faculty.Status = FacultyStatus.InActive;

                var result = await _dbContext.SaveChangesAsync(token);

                return result > 0
                    ? new Response<string>(HttpStatusCode.OK, "Data successfully deleted !")
                    : new Response<string>(HttpStatusCode.InternalServerError, "Data not deleted !");
            }
            catch (Exception ex)
            {
                return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<PagedResponse<List<GetFacultyDto>>> GetAllFaculties(FacultyFilter filter, CancellationToken token = default)
        {
            try
            {
                var validFilter = new FacultyFilter(filter.PageNumber, filter.PageSize);

                var query = _dbContext.Faculties.OrderBy(x=>x.Id).AsQueryable();

                if (filter.Name != null)
                {
                    query = query.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
                }

                var totalRecords = await query.AsNoTracking().CountAsync(token);

                var faculties = await query.Select(faculty => new GetFacultyDto
                {
                    Id = faculty.Id,
                    Name = faculty.Name,
                    Status = faculty.Status,
                    Description = faculty.Description
                }).Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize)
                  .AsNoTracking().ToListAsync(token);

                return new PagedResponse<List<GetFacultyDto>>(
                    faculties, HttpStatusCode.OK, "All faculties",
                    totalRecords, validFilter.PageNumber, validFilter.PageSize);
            }
            catch (Exception ex)
            {
                return new PagedResponse<List<GetFacultyDto>>(HttpStatusCode.InternalServerError, ex.Message, 0, 0, 0);
            }
        }

        public async Task<Response<GetFacultyDto>> GetFacultyById(int facultyId, CancellationToken token = default)
        {
            try
            {
                var faculty = await _dbContext.Faculties.AsNoTracking().FirstOrDefaultAsync(x => x.Id == facultyId);
                if (faculty == null) return new Response<GetFacultyDto>(HttpStatusCode.NotFound, "Faculty not found !");

                var model = new GetFacultyDto
                {
                    Id = faculty.Id,
                    Name = faculty.Name,
                    Status = faculty.Status,
                    Description = faculty.Description
                };

                return new Response<GetFacultyDto>(HttpStatusCode.OK, "Faculty found !", model);
            }
            catch (Exception ex)
            {
                return new Response<GetFacultyDto>(HttpStatusCode.OK,ex.Message);
            }
        }

        public async Task<Response<UpdateFacultyDto>> UpdateFaculty(UpdateFacultyDto model, CancellationToken token = default)
        {
            try
            {
                var faculty = await _dbContext.Faculties.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (faculty == null) return new Response<UpdateFacultyDto>(HttpStatusCode.NotFound, "Faculty not found !");

                faculty.Name = model.Name;
                faculty.Description = model.Description;
                faculty.Status = model.Status;

                // _dbContext.Faculties.Update(faculty);
                var result = await _dbContext.SaveChangesAsync(token);

                return result > 0
                    ? new Response<UpdateFacultyDto>(HttpStatusCode.OK, "Data successfully updated !")
                    : new Response<UpdateFacultyDto>(HttpStatusCode.InternalServerError, "Data not updated !");
            }
            catch (Exception ex)
            {
                return new Response<UpdateFacultyDto>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}

