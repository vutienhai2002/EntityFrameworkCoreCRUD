using EntityFrameworkCoreCRUD.Models;

namespace EntityFrameworkCoreCRUD.Service
{
    public interface IBuyerService
    {

        Buyer GetBuyerById(int id);
        bool IsEmailExists(string email);
        void AddBuyer(Buyer buyer);
        void UpdateBuyer(Buyer buyer);
    }
}
