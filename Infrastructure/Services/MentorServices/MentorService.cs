namespace Infrastructure
{
    public class MentorService : IMentorService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileService _fileService;
        public MentorService(
            ApplicationDbContext dbContext,
            IFileService fileService)
        {
            _dbContext = dbContext;
            _fileService = fileService;
        }
        public async Task<Response<AddMentorDto>> AddMentor(AddMentorDto model, CancellationToken token = default)
        {
            var mentor = await _dbContext.Mentors.AsNoTracking().FirstOrDefaultAsync(x=>x.Phone.Trim()==model.Phone.Trim(),token);
            if (mentor != null) return new Response<AddMentorDto>(HttpStatusCode.Found, "Mentor already exists with this number !");

            var newMentor = new Mentor
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Status = model.Status,
                CreateDate = DateTime.UtcNow,
                Email = model.Email,
                Gender = model.Gender,
                Phone = model.Phone,
                FileName = model.FileName!=null?await _fileService.AddFileAsync(FolderType.Image,model.FileName):"default.png"
            };

            await _dbContext.Mentors.AddAsync(newMentor,token);
            var result = await _dbContext.SaveChangesAsync(token);

            return result > 0
                ? new Response<AddMentorDto>(HttpStatusCode.OK, "Data successfully added !")
                : new Response<AddMentorDto>(HttpStatusCode.InternalServerError, "Data not added !");
        }

        public async Task<Response<string>> DeleteMentorById(int mentorId, CancellationToken token = default)
        {
            var mentor = await _dbContext.Mentors.FirstOrDefaultAsync(x => x.Id==mentorId, token);
            if (mentor == null) return new Response<string>(HttpStatusCode.NotFound, "Mentor not found !");

            /*if(mentor.FileName!= "default.png")
            {
                _ = Task.Run(async () =>
                {
                    await _fileService.DeleteFileAsync(FolderType.Image, mentor.FileName);
                });
            }
            
            _dbContext.Mentors.Remove(mentor);
            */
            mentor.Status = MentorStatus.InActive;
            var result = await _dbContext.SaveChangesAsync(token);

            return result > 0
                ? new Response<string>(HttpStatusCode.OK, "Data successfully added !")
                : new Response<string>(HttpStatusCode.InternalServerError, "Data not added !");
        }

        public async Task<PagedResponse<List<GetMentorDto>>> GetAllMentors(MentorFilter filter, CancellationToken token = default)
        {
            var validFilter = new MentorFilter(filter.PageNumber, filter.PageSize);

            var query = _dbContext.Mentors.OrderBy(x=>x.Id).AsQueryable();

            if(filter.Name!=null)
            {
                query = query.Where(x=>x.FirstName.ToLower().Trim().Contains(filter.Name.ToLower().Trim())||
                                       x.LastName.ToLower().Trim().Contains(filter.Name.ToLower().Trim()));
            }
            if(filter.Phone!=null)
            {
                query = query.Where(x => x.Phone==filter.Phone);
            }

            var totalRecords = await query.AsNoTracking().CountAsync(token);

            var mentors = await query.Select(mentor => new GetMentorDto
            {
                Id = mentor.Id,
                FullName = mentor.LastName+" "+mentor.FirstName,
                Email = mentor.Email,
                FileName = mentor.FileName,
                Status = mentor.Status,
                Gender = mentor.Gender,
                Phone = mentor.Phone
            }).Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize)
            .AsNoTracking().ToListAsync();

            return new PagedResponse<List<GetMentorDto>>(mentors, HttpStatusCode.OK, "All mentors",
                totalRecords, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<GetMentorDetailDto>> GetMentorById(int mentorId, CancellationToken token = default)
        {
            var mentor = await _dbContext.Mentors.AsNoTracking().FirstOrDefaultAsync(x => x.Id == mentorId, token);
            if (mentor == null) return new Response<GetMentorDetailDto>(HttpStatusCode.NotFound, "Mentor not found !");

            var model = new GetMentorDetailDto
            {
                Id = mentor.Id,
                FirstName = mentor.FirstName,
                LastName = mentor.LastName,
                Email = mentor.Email,
                FileName = mentor.FileName,
                Status = mentor.Status,
                Gender = mentor.Gender,
                Phone = mentor.Phone
            };

            return new Response<GetMentorDetailDto>(HttpStatusCode.OK, "Mentor successfully found !",model);
        }

        public async Task<Response<UpdateMentorDto>> UpdateMentor(UpdateMentorDto model, CancellationToken token = default)
        {
            var mentor = await _dbContext.Mentors.FirstOrDefaultAsync(x => x.Id == model.Id, token);
            if (mentor == null) return new Response<UpdateMentorDto>(HttpStatusCode.NotFound, "Mentor not found !");

            mentor.FirstName = model.FirstName;
            mentor.LastName = model.LastName;
            mentor.Status = model.Status;
            mentor.UpdateDate = DateTime.UtcNow;
            mentor.Email = model.Email;
            mentor.Gender = model.Gender;
            mentor.Phone = model.Phone;
            mentor.FileName = model.FileName != null ? await _fileService.AddFileAsync(FolderType.Image, model.FileName) : mentor.FileName;
          
            var result = await _dbContext.SaveChangesAsync(token);

            return result > 0
                ? new Response<UpdateMentorDto>(HttpStatusCode.OK, "Data successfully updated !")
                : new Response<UpdateMentorDto>(HttpStatusCode.InternalServerError, "Data not added !");
        }
    }
}
