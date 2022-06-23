using System.Threading.Tasks;
using SchoolBackOffice.Application.Common.Models;

namespace SchoolBackOffice.Application.Common.Interfaces
{
    public interface IStaffUserViewModelService
    {
        Task<(EditStaffViewModelDto EditStaffDto, string Error)> GetEditStaffViewModel(int staffUserId);
    }
}