using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();

        bool CreateMember (CreateMemberViewModel CreatedMember);

        MemberViewModel? GetMemberDetails(int MemberId);

        HealthRecordViewModel? GetMemberHealthRecord(int MemberId);

        MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);

        bool UpdateMemberDetails ( int Id ,  MemberToUpdateViewModel UpdatedMember);

        bool RemoveMember(int MemberId);








    }
}
