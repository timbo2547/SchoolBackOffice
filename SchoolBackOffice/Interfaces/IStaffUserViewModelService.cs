using System.Threading.Tasks;
using SchoolBackOffice.ViewModels;

namespace SchoolBackOffice.Interfaces
{
    public interface IStaffUserViewModelService
    {
        Task<(EditStaffViewModel EditStaffDto, string Error)> GetEditStaffViewModel(int staffUserId);

    }
}