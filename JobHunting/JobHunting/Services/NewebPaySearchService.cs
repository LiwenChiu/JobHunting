using JobHunting.Models;

namespace JobHunting.Services
{
    public class NewebPaySearchService
    {
        private readonly DuckContext _context;

        public NewebPaySearchService(DuckContext context)
        {
            _context = context;
        }

        public async Task NewebPaySearchStatusFalse()
        {
            var companyOrders = _context.CompanyOrders.Where(co => !co.Status);
        }


    }
}
