using Microsoft.AspNetCore.Mvc;
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
                 Name = "Toyota"
             },
             new VehicleModel
             {
                 Id  = 2,
                 Name = "Vios"
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
    }
}
