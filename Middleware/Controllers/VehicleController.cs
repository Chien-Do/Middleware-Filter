using Microsoft.AspNetCore.Mvc;
using Middleware.ModelBinding;
using Middleware.Models;
using System.Collections.Generic;
using System.Linq;


namespace Middleware.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly List<VehicleModel> _vehicles;

        public VehicleController()
        {
            _vehicles = new List<VehicleModel> {
             new VehicleModel
             {
                 Id = 1,
                 Name = "Toyota",
                 CreatedDate  = new System.DateTime(2022,01,01)
             },
             new VehicleModel
             {
                 Id  = 2,
                 Name = "Vios",
                 CreatedDate  = new System.DateTime(2021,01,01)
             }

            };
        }
        [HttpGet]
        public List<VehicleModel> GetAllVehicles()
        {
            return _vehicles;
        }


        [HttpGet("detail")]
        public VehicleModel GetVehicleById([FromQuery] int id)
        {
            return _vehicles.FirstOrDefault(r => r.Id == id);
        }

        [HttpGet("getDateTime")]
        public List<VehicleModel> GetVehicleByCreatedDate(DateTimeViewModel viewModel)
        {
            if (viewModel == null)
                return new List<VehicleModel>();


            return _vehicles.Where(r => r.CreatedDate == viewModel.MyDate).ToList();

        }

    }
}
