using ThanalSoft.SmartComplex.Business.Repositories;
using ThanalSoft.SmartComplex.DataAccess;
using ThanalSoft.SmartComplex.Entities.Security;

namespace ThanalSoft.SmartComplex.Business.User
{
    public class UserRepository : RepositoryService<LoginUser>, IUserRepository
    {
        public UserRepository(SmartComplexDataObjectContext pContext) : base(pContext)
        {
        }
    }
}