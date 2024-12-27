using CoreAPI_V3.CommonMethods;
using CoreAPI_V3.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Claims;
using static CoreAPI_V3.Models.DashboardModel;

namespace CoreAPI_V3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuoyDataController : ControllerBase
    {
        private readonly string _config;
        private readonly CommonHelper _commonHelper;
        private readonly DashboardRepo _repo;

        public BuoyDataController(IConfiguration config, CommonHelper commonHelper, DashboardRepo repo)
        {
            _config = config.GetConnectionString("DefaultConnection");
            _commonHelper = commonHelper;
            _repo = repo;   
        }


        [Authorize]
        [HttpGet("GetBuoyInfoDateRange")]
        public IActionResult GetBuoyInfoDateRange([FromQuery] string stationId, [FromQuery] string date, string time) 
        {
            ResponseMessages ResponseStatus = new ResponseMessages();

            try
            {
                var buoyData = _repo.GetBuoyInfoDateRange(stationId, date, time);

                if (buoyData != null)
                {
                    return Ok(buoyData);
                }
                else
                {
                    ResponseStatus = CommonHelper.NoContent("No data found");
                    return StatusCode(StatusCodes.Status204NoContent, new { ResponseStatus });
                }
            }
            catch (Exception ex)
            {
                _commonHelper.SendErrorToText("Controller", "Dashboard", "GetBuoyInfoDateRange", ex);
                ResponseStatus = CommonHelper.NoContent(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseStatus });
            }
        }


        [Authorize]   
        [HttpGet("GetAllBuoysList")]
        public  IActionResult GetAllBuoysList(bool is_all)     
        {
            ResponseMessages ResponseStatus = new ResponseMessages();
            try
            {
                var GetAllBuoysResp = _repo.GetAllBuoys(is_all);

                if (GetAllBuoysResp != null && GetAllBuoysResp.Count > 0) 
                {
                    if (!GetAllBuoysResp[0].is_success) 
                    {
                        ResponseStatus = CommonHelper.Success("Buoys List Available");
                    }
                    else
                    {
                        if (is_all)
                        {
                            GetAllBuoysResp = null;
                            ResponseStatus = CommonHelper.NoContent("Buoys List Not Available");
                        }
                        else
                        {
                            ResponseStatus = CommonHelper.Success("Buoys List Available");
                        }
                    }
                }
                else
                {
                    GetAllBuoysResp = null;

                    ResponseStatus = CommonHelper.NoContent("Buoys List Not Available");
                    return StatusCode(StatusCodes.Status204NoContent, new { ResponseStatus });
                }

                return Ok(new { GetAllBuoysResp, ResponseStatus });
            }
            catch (Exception ex)
            {
                _commonHelper.SendErrorToText("Controller", "Dashboard", "GetAllBuoysList", ex);
                ResponseStatus = CommonHelper.NoContent(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseStatus });
            }
        }


        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("GetBuoyDataNew")]
        public async Task<IActionResult> GetBuoyDataNew()
        {
            //if (!User.IsInRole("Admin"))
            //{
            //    return Forbid("You are not authorized to access this resource.");
            //}
            ResponseMessages ResponseStatus = new ResponseMessages();
            try
            {

                var col_name = "";
                var GetBuoyDataNewRepo = _repo.GetBuoyDataNew(col_name);
                var GetTotalBuoyData =  _repo.GetTotalBuoyDataNew(col_name);
                var getLink =  _repo.GetLink();

                if (GetBuoyDataNewRepo != null && GetTotalBuoyData != null)
                {
                    return Ok(new { GetBuoyDataNewRepo, GetTotalBuoyData, getLink });
                }
                else
                {
                    ResponseStatus = CommonHelper.NoContent("No data found");
                    return StatusCode(StatusCodes.Status204NoContent, new { ResponseStatus });
                }
            }
            catch (Exception ex)
            {
                _commonHelper.SendErrorToText("Controller", "Dashboard", "GetBuoyDataNew", ex);
                ResponseStatus = CommonHelper.NoContent(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseStatus });
            }
        }


        [Authorize]
        [HttpGet("GetMinMax")]
        public async Task<IActionResult> GetBuoyMinMaxParamValues(string stationId)
        {
            ResponseMessages ResponseStatus = new ResponseMessages();

            try
            {
                var result = _repo.GetBuoyMinMaxParamValues(stationId);
                if(result != null)
                {
                    return Ok(result);
                }
                else
                {
                     ResponseStatus = CommonHelper.NoContent("No data found");
                    return StatusCode(StatusCodes.Status204NoContent, new { ResponseStatus });
                }
            }
            catch(Exception ex)
            {
                _commonHelper.SendErrorToText("Controller", "Dashboard", "GetBuoyMinMaxParamValues", ex);
                ResponseStatus = CommonHelper.NoContent(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseStatus });
            }
        }




    }
}
