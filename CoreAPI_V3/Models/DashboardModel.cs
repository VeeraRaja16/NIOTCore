namespace CoreAPI_V3.Models
{
    public class DashboardModel
    {
        public class Dashboard24Details : updateQuery
        {
            public string? StationId { get; set; }
            public string? BuoyType { get; set; }
            public string? CpuDateTime { get; set; }


            public string? AirPressure { get; set; }
            public string? AirTemperature { get; set; }
            public string? AirHumidity { get; set; }
            public string? WindSpeed { get; set; }
            public string? WindGust { get; set; }
            public string? WindDirection { get; set; }
            public string? Hm0 { get; set; }
            public string? Hmax { get; set; }
            public string? Rainfall { get; set; }
            public string? Tz { get; set; }
            public string? Thmax { get; set; }

            public string? Pressure500m { get; set; }

            public string? WaterTemperature { get; set; } // DB
            public string? WaterConductivity { get; set; } // DB



            public string? CurrSp015 { get; set; }  
            public string? CurrDir015 { get; set; }  
            public string? CurrSp025 { get; set; }
            public string? CurrDir025 { get; set; }
            public string? CurrSp035 { get; set; }
            public string? CurrDir035 { get; set; }
            public string? CurrSp050 { get; set; }
            public string? CurrDir050 { get; set; }
            public string? CurrSp075 { get; set; }
            public string? CurrDir075 { get; set; }
            public string? CurrSp100 { get; set; }
            public string? CurrDir100 { get; set; }

            public string? Temperature010 { get; set; }  
            public string? Temperature015 { get; set; }
            public string? Temperature020 { get; set; }
            public string? Temperature030 { get; set; }
            public string? Temperature050 { get; set; }
            public string? Temperature075 { get; set; }
            public string? Temperature100 { get; set; }
            public string? Temperature200 { get; set; }
            public string? Temperature500m { get; set; }

            public string? Conductivity5m { get; set; }  
            public string? Conductivity010 { get; set; }
            public string? Conductivity015 { get; set; }
            public string? Conductivity020 { get; set; }
            public string? Conductivity030 { get; set; }
            public string? Conductivity050 { get; set; }
            public string? Conductivity075 { get; set; }
            public string? Conductivity100 { get; set; }
            public string? Conductivity200 { get; set; }
            public string? Conductivity500m { get; set; }
            public string? Conductivity1m { get; set; } // upto this all above params common for OB and DB
           


            public string? Salinity010 { get; set; } // Salinity only for OB not DB and TB
            public string? Salinity015 { get; set; }
            public string? Salinity020 { get; set; }
            public string? Salinity030 { get; set; }
            public string? Salinity050 { get; set; }
            public string? Salinity075 { get; set; }
            public string? Salinity100 { get; set; }
            public string? Salinity200 { get; set; }
            public string? Salinity500 { get; set; }

            public string? Mode { get; set; }
           // public bool? is_success {get; set; }
        }
        public class updateQuery
        {
            public string? WaterLevel1 { get; set; }
            public string? WaterLevel2 { get; set; }
            public string? WaterLevel3 { get; set; }
            public string? WaterLevel4 { get; set; }
            public string? WaterLevel5 { get; set; }
            public string? WaterLevel6 { get; set; }
            public string? WaterLevel7 { get; set; }
            public string? WaterLevel8 { get; set; }
            public string? WaterLevel9 { get; set; }
            public string? WaterLevel10 { get; set; }
        }

        public class ResponseMessages
        {
            public int statuscode { get; set; }
            public string status { get; set; }
            public string message { get; set; }
        }


        public class CommonResponse
        { 
            public bool is_success { get; set; }
        }

        public class GetAllBouysResp : CommonResponse
        {

            public string stationid { get; set; }

            public string s_status { get; set; }

            public string b_buoyid { get; set; }
            public string s_type { get; set; }
            
            public string s_type_s_id { get; set; }

            public string b_latitude { get; set; }

            public string b_longitude { get; set; }

            public string dep_latitude { get; set; }

            public string dep_longitude { get; set; }

            public DateTime? b_depdt { get; set; }
            public DateTime? b_retrivaldate { get; set; }
            public int? no_of_days { get; set; }

            public bool? is_station_actv { get; set; }

            public string? dispaly_b_latitude { get; set; }

            public string? dispaly_b_longitude { get; set; }

            public string? dispaly_dep_latitude { get; set; }

            public string? dispaly_dep_longitude { get; set; }

            public string? color_code { get; set; }

        }

        public class BuoysStatusResp : CommonResponse
        {
            public string stationid { get; set; }
            public bool? is_station_actv { get; set; }
        }



        #region Functional Buoy Status
        public class GetBuoyDataNew : CommonResponse
        {
            public string s_type { get; set; }
            public string BUOYLINK { get; set; }
            public string STATION_ID { get; set; }
            public string DEPLOYMENT_DATE { get; set; }
            public string STATUS { get; set; }
            public string No_of_Days_at_Sea { get; set; }
            public string Days_Worked_at_Sea { get; set; }
            public string Last_transmissionDate { get; set; }
            public string BS_REMARKS { get; set; }

        }

        public class GetTotalBuoyDataNew : CommonResponse
        {
            public string StationType { get; set; }
            public string WorkingCount { get; set; }
            public string NotWorkingCount { get; set; }
        }

        public class Link
        {
            public string BS_LINK { get; set; }
        }

        #endregion





        public class BuoyTypeResp : CommonResponse
        {
            public string stationid { get; set; }
            public string s_type { get; set; }
            public bool is_station_actv { get; set; }
        }





        public class BuoyMinMaxResp : CommonResponse
        {
            //public string min_battery { get; set; }
            //public string max_battery { get; set; }
            public string min_airpressure { get; set; }

            public string max_airpressure { get; set; }

            public string min_airtemperature { get; set; }

            public string max_airtemperature { get; set; }

            public string min_airhumidity { get; set; }

            public string max_airhumidity { get; set; }

            public string min_rainfall { get; set; }

            public string max_rainfall { get; set; }

            public string min_hm0 { get; set; }

            public string max_hm0 { get; set; }

            public string min_hmax { get; set; }

            public string max_hmax { get; set; }

            public string min_tz { get; set; }

            public string max_tz { get; set; }

            public string min_thmax { get; set; }

            public string max_thmax { get; set; }

            public string min_pressure500m { get; set; }

            public string max_pressure500m { get; set; }

            public string min_temperature { get; set; }

            public string max_temperature { get; set; }

            public string min_conductivity { get; set; }

            public string max_conductivity { get; set; }

            public string min_db_temperature { get; set; }

            public string max_db_temperature { get; set; }

            public string min_db_conductivity { get; set; }

            public string max_db_conductivity { get; set; }

            public string min_windspeed { get; set; }

            public string max_windspeed { get; set; }

            public string min_currsp { get; set; }

            public string max_currsp { get; set; }
        }



    }


}
