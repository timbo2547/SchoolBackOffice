using System.Threading.Tasks;
using SchoolBackOffice.ViewModels;

namespace SchoolBackOffice.Interfaces
{
    public interface IStudentUserViewModelService
    {
        Task<(EditStudentViewModel EditStudentViewModel, string Error)> GetEditStudentViewModel(int studentUserId);

    }
}