using Azure.Core;
using CoreAPI_V3.CommonMethods;
using CoreAPI_V3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Owin.BuilderProperties;
using Microsoft.Owin.Security.Provider;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Security.Claims;
using static CoreAPI_V3.Models.DashboardModel;

namespace CoreAPI_V3.Repository
{
    public class DashboardRepo
    {
        private readonly string _config;
        private readonly CommonHelper _commonHelper;
        private readonly ApplicationDbContext _context;

        public DashboardRepo(IConfiguration config, CommonHelper commonHelper, ApplicationDbContext context)
        {
            _config = config.GetConnectionString("DefaultConnection");
            _commonHelper = commonHelper;
            _context = context;
        }

        public List<Dashboard24Details> GetBuoyInfoDateRange(string stationId, string date, string time)
        {
            using (SqlConnection con = new SqlConnection(_config))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("Sp_GetMob_6_BuoyInfoDateRange", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@stationid", stationId);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@Time", time);

                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd != null)
                        {
                            var buoyData = new List<Dashboard24Details>();

                            while (rd.Read())
                            {
                                var data = new Dashboard24Details
                                {

                                    StationId = rd["stationid"].ToString(),
                                    BuoyType = rd["buoytype"].ToString(),
                                    CpuDateTime = rd["cpudatetime"].ToString(),

                                    AirPressure = rd["airpressure"].ToString(),
                                    AirTemperature = rd["airtemperature"].ToString(),
                                    AirHumidity = rd["airhumidity"].ToString(),
                                    WindSpeed = rd["windspeed"].ToString(),
                                    WindGust = rd["windGust"].ToString(),
                                    WindDirection = rd["winddirection"].ToString(),
                                    Hm0 = rd["hm0"].ToString(),
                                    Hmax = rd["hmax"].ToString(),
                                    Rainfall = rd["rainfall"].ToString(),
                                    Tz = rd["tz"].ToString(),
                                    Thmax = rd["thmax"].ToString(),
                                    Pressure500m = rd["pressure500m"].ToString(),
                                    WaterTemperature = rd["WaterTemperature"].ToString(),
                                    WaterConductivity = rd["WaterConductivity"].ToString(),


                                    CurrSp015 = rd["CurrSp015"].ToString(),
                                    CurrDir015 = rd["CurrDir015"].ToString(),
                                    CurrSp025 = rd["CurrSp025"].ToString(),
                                    CurrDir025 = rd["CurrDir025"].ToString(),
                                    CurrSp035 = rd["CurrSp035"].ToString(),
                                    CurrDir035 = rd["CurrDir035"].ToString(),
                                    CurrSp050 = rd["CurrSp050"].ToString(),
                                    CurrDir050 = rd["CurrDir050"].ToString(),
                                    CurrSp075 = rd["CurrSp075"].ToString(),
                                    CurrDir075 = rd["CurrDir075"].ToString(),
                                    CurrSp100 = rd["CurrSp100"].ToString(),
                                    CurrDir100 = rd["CurrDir100"].ToString(),


                                    Temperature010 = rd["Temperature010"].ToString(),
                                    Temperature015 = rd["Temperature015"].ToString(),
                                    Temperature020 = rd["Temperature020"].ToString(),
                                    Temperature030 = rd["Temperature030"].ToString(),
                                    Temperature050 = rd["Temperature050"].ToString(),
                                    Temperature075 = rd["Temperature075"].ToString(),
                                    Temperature100 = rd["Temperature100"].ToString(),
                                    Temperature200 = rd["Temperature200"].ToString(),
                                    Temperature500m = rd["Temperature500m"].ToString(),

                                    Conductivity5m = rd["Conductivity5m"].ToString(),
                                    Conductivity1m = rd["Conductivity1m"].ToString(),
                                    Conductivity010 = rd["Conductivity010"].ToString(),
                                    Conductivity015 = rd["Conductivity015"].ToString(),
                                    Conductivity020 = rd["Conductivity020"].ToString(),
                                    Conductivity030 = rd["Conductivity030"].ToString(),
                                    Conductivity050 = rd["Conductivity050"].ToString(),
                                    Conductivity075 = rd["Conductivity075"].ToString(),
                                    Conductivity100 = rd["Conductivity100"].ToString(),
                                    Conductivity200 = rd["Conductivity200"].ToString(),
                                    Conductivity500m = rd["Conductivity500m"].ToString(),


                                    Salinity010 = rd["Salinity010"].ToString(),
                                    Salinity015 = rd["Salinity015"].ToString(),
                                    Salinity020 = rd["Salinity020"].ToString(),
                                    Salinity030 = rd["Salinity030"].ToString(),
                                    Salinity050 = rd["Salinity050"].ToString(),
                                    Salinity075 = rd["Salinity075"].ToString(),
                                    Salinity100 = rd["Salinity100"].ToString(),
                                    Salinity200 = rd["Salinity200"].ToString(),
                                    Salinity500 = rd["Salinity500"].ToString(),


                                    WaterLevel1 = rd["water_level1"].ToString(),
                                    WaterLevel2 = rd["water_level2"].ToString(),
                                    WaterLevel3 = rd["water_level3"].ToString(),
                                    WaterLevel4 = rd["water_level4"].ToString(),
                                    WaterLevel5 = rd["water_level5"].ToString(),
                                    WaterLevel6 = rd["water_level6"].ToString(),
                                    WaterLevel7 = rd["water_level7"].ToString(),
                                    WaterLevel8 = rd["water_level8"].ToString(),
                                    WaterLevel9 = rd["water_level9"].ToString(),
                                    WaterLevel10 = rd["water_level10"].ToString(),
                                    Mode = rd["mode"].ToString(),

                                };
                                buoyData.Add(data);
                            }
                            return buoyData;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }


        public List<GetAllBouysResp> GetAllBuoys(bool is_all)
        {
            List<GetAllBouysResp> GetAllBouysResp = new List<GetAllBouysResp>();
            List<BuoysStatusResp> BuoysStatusResp = new List<BuoysStatusResp>();
            try
            {
                using (var defaultCon = new SqlConnection(_config))
                {
                    using (var cmd = new SqlCommand("SP_GetMob_AllBuoysStatus", defaultCon))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        defaultCon.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var buoysStatusResp = new BuoysStatusResp
                                {
                                    stationid = reader["stationid"]?.ToString(),
                                    is_success = Convert.ToBoolean(reader["is_success"]),
                                    is_station_actv = Convert.ToBoolean(reader["is_station_actv"])
                                };

                                BuoysStatusResp.Add(buoysStatusResp);
                            }
                        }
                    }
                }

                using (var storeCon = new SqlConnection(_config))
                {
                    using (var cmd = new SqlCommand("SP_GetMobAllBuoysList", storeCon))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@is_all", is_all));
                        storeCon.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var buoyResp = new GetAllBouysResp
                                {
                                    stationid = reader["stationid"]?.ToString(),
                                    s_status = reader["s_status"]?.ToString(),
                                    b_buoyid = reader["b_buoyid"]?.ToString(),
                                    s_type = reader["s_type"]?.ToString(),
                                    b_latitude = reader["b_latitude"]?.ToString(),
                                    b_longitude = reader["b_longitude"]?.ToString(),
                                    b_depdt = reader["b_depdt"] != DBNull.Value ? (DateTime?)reader["b_depdt"] : null,
                                    b_retrivaldate = reader["b_retrivaldate"] != DBNull.Value ? (DateTime?)reader["b_retrivaldate"] : null,
                                    no_of_days = reader["no_of_days"] != DBNull.Value ? (int?)reader["no_of_days"] : null,
                                    is_success = Convert.ToBoolean(reader["is_success"]),
                                    dep_latitude = null,
                                    dep_longitude = null,
                                    dispaly_b_latitude = null,
                                    dispaly_b_longitude = null,
                                    dispaly_dep_latitude = null,
                                    dispaly_dep_longitude = null,
                                    color_code = null,

                                    s_type_s_id = reader["s_type"]?.ToString() + "-" + reader["stationid"]?.ToString()

                                }; 

                                GetAllBouysResp.Add(buoyResp);
                            }
                        }
                    }
                }

                var random = new Random();
                if (GetAllBouysResp.Count > 0)
                {
                    foreach (GetAllBouysResp buoy in GetAllBouysResp)
                    {
                        if (BuoysStatusResp.Count > 0 && BuoysStatusResp[0].is_success)
                        {
                            var status = BuoysStatusResp.FirstOrDefault(b => b.stationid == buoy.stationid);
                            if (status != null)
                            {
                                buoy.is_station_actv = status.is_station_actv;
                            }
                            else
                            {
                                status.is_success = false;

                            }
                            
                        }
                         
                        buoy.dispaly_b_latitude = buoy.b_latitude.Replace("D", "° ").Replace("M", "’ ").Replace("S", "") + " (+)N/(-)S";
                        buoy.dispaly_b_longitude = buoy.b_longitude.Replace("D", "° ").Replace("M", "’ ").Replace("S", "") + " E";
                       

                        var retmsg = "";
                        if (buoy.stationid == "SBIRZ")
                        {
                            buoy.dep_latitude = CommonHelper.Getlatlongb(buoy.b_latitude, ref retmsg);
                            buoy.dep_longitude = CommonHelper.Getlatlong(buoy.b_longitude, ref retmsg);
                        }
                        else
                        {
                            buoy.dep_latitude = CommonHelper.Getlatlong(buoy.b_latitude, ref retmsg);
                            buoy.dep_longitude = CommonHelper.Getlatlong(buoy.b_longitude, ref retmsg);
                        }


                         buoy.color_code = String.Format("#{0:X6}", random.Next(0x1000000));

                    }
                }
            }
            catch (Exception ex)
            {
                _commonHelper.SendErrorToText("Repo", "Dashboard", "GetAllBuoys", ex);
                if (GetAllBouysResp.Count > 0)
                {
                    GetAllBouysResp[0].is_success = false;
                }
            }

            return GetAllBouysResp;
        }



        #region Active or Not
        public List<GetBuoyDataNew> GetBuoyDataNew(string col_name) 
        {
            List<GetBuoyDataNew> getBuoyDataNew = new List<GetBuoyDataNew>();

            try
            {
                using(SqlConnection con = new SqlConnection(_config))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_GetBuoyStatus_NEW", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        var rd =  cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            var BuoyData = new GetBuoyDataNew
                            {
                                s_type = rd["s_type"].ToString(),
                                BUOYLINK = rd["BUOYLINK"].ToString(),
                                STATION_ID = rd["STATION_ID"].ToString(),
                                DEPLOYMENT_DATE = rd["DEPLOYMENT_DATE"].ToString(),
                                STATUS = rd["STATUS"].ToString(),
                                No_of_Days_at_Sea = rd["No_of_Days_at_Sea"].ToString(),
                                Days_Worked_at_Sea = rd["Days_Worked_at_Sea"].ToString(),
                                Last_transmissionDate = rd["Last_transmissionDate"].ToString(),
                                BS_REMARKS = rd["BS_REMARKS"].ToString(),
                                is_success = Convert.ToBoolean(rd["is_success"])

                            };
                            getBuoyDataNew.Add(BuoyData);
                        } 
                    }
                }
            }
            catch(Exception ex)
            {
                _commonHelper.SendErrorToText("Repo", "Dashboard", "GetBuoyDataNew", ex);
            }
            return getBuoyDataNew;

        }


        public List<GetTotalBuoyDataNew> GetTotalBuoyDataNew(string col_name)
        {
            List<GetTotalBuoyDataNew> getTotalBuoyDataNew = new List<GetTotalBuoyDataNew>();

            try
            {
                using (SqlConnection con = new SqlConnection(_config))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetBuoyStatusTotal_NEW", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        con.Open();
                        var rd = cmd.ExecuteReader();

                        while (rd.Read()) 
                        {
                            var buoyTotal = new GetTotalBuoyDataNew
                            {
                                StationType = rd["StationType"].ToString(),
                                WorkingCount = rd["WorkingCount"].ToString(),
                                NotWorkingCount = rd["NotWorkingCount"].ToString(),
                                is_success = Convert.ToBoolean(rd["is_success"])

                            };
                            getTotalBuoyDataNew.Add(buoyTotal);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _commonHelper.SendErrorToText("Repo", "Dashboard", "GetTotalBuoyDataNews", ex);

            }

            return getTotalBuoyDataNew;
        }



        public List<Link> GetLink()
        {

            List<Link> getLinks = new List<Link>();
            try
            {
                using (SqlConnection con = new SqlConnection(_config))
                {
                    var stringQuery = "select top 1  BS_LINK from ADDRESS2_BUOY_STATUS where CONVERT(date, BS_CREATEDDATE, 105) = CONVERT(date, GETDATE(), 105) and BS_LINK !='' and BS_LINK is not null order by BS_CREATEDDATE desc";

                    SqlCommand cmd = new SqlCommand(stringQuery, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    con.Open();

                   var rd =  cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        var link = new Link
                        {
                            BS_LINK = rd["BS_LINK"].ToString()
                        };
                        getLinks.Add(link);
                    }
                   return getLinks;

                }
            }
            catch(Exception ex )
            {
                _commonHelper.SendErrorToText("Repo", "Dashboard", "GetLink", ex);
            }

            return null;
        }


      

        #endregion




       // get Min and Max

        public List<BuoyMinMaxResp> GetBuoyMinMaxParamValues(string stationId)
        {
            var list = new List<BuoyMinMaxResp>();
            try
            {
                using (var con = new SqlConnection(_config))
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_GetBuoymin_maxData", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@stationid", stationId);
                        con.Open();
                        var rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            var lst = new BuoyMinMaxResp
                            {
                                min_airpressure = rd["min_airpressure"].ToString(),
                                max_airpressure = rd["max_airpressure"].ToString(),
                                min_airtemperature = rd["min_airtemperature"].ToString(),
                                max_airtemperature = rd["max_airtemperature"].ToString(),
                                min_airhumidity = rd["min_airhumidity"].ToString(),
                                max_airhumidity = rd["max_airhumidity"].ToString(),
                                min_rainfall = rd["min_rainfall"].ToString(),
                                max_rainfall = rd["max_rainfall"].ToString(),
                                min_hm0 = rd["min_hm0"].ToString(),
                                max_hm0 = rd["max_hm0"].ToString(),
                                min_hmax = rd["min_hmax"].ToString(),
                                max_hmax = rd["max_hmax"].ToString(),
                                min_tz = rd["min_tz"].ToString(),
                                max_tz = rd["max_tz"].ToString(),
                                min_thmax = rd["min_thmax"].ToString(),
                                max_thmax = rd["max_thmax"].ToString(),
                                min_pressure500m = rd["min_pressure500m"].ToString(),
                                max_pressure500m = rd["max_pressure500m"].ToString(),
                                min_temperature = rd["min_temperature"].ToString(),
                                max_temperature = rd["max_temperature"].ToString(),
                                min_conductivity = rd["min_conductivity"].ToString(),
                                max_conductivity = rd["max_conductivity"].ToString(),
                                min_db_temperature = rd["min_db_temperature"].ToString(),
                                max_db_temperature = rd["max_db_temperature"].ToString(),
                                min_db_conductivity = rd["min_db_conductivity"].ToString(),
                                max_db_conductivity = rd["max_db_conductivity"].ToString(),
                                min_windspeed = rd["min_windspeed"].ToString(),
                                max_windspeed = rd["max_windspeed"].ToString(),
                                min_currsp = rd["min_currsp"].ToString(),
                                max_currsp = rd["max_currsp"].ToString(),
                                is_success = true,

                            };
                            list.Add(lst);
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                list[0].is_success = false;
                return null;
            }

        }

    }
}
