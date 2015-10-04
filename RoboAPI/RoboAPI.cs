using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace RoboAPI
{
    class RoboAPI
    {

        private static readonly ILog log = log4net.LogManager.GetLogger("RoboAPILog");

        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "/?")
            {
                Console.WriteLine("Inform filename with ids.");
                return;
            }

            int counterOk = 0;
            int counterNok = 0;
            string line;
            string id;
            string json;

            try
            {
                log4net.Config.XmlConfigurator.Configure();
                System.IO.StreamReader file = new System.IO.StreamReader(args[0]);

                try
                {
                    log.Info("Starting...");
                    while ((line = file.ReadLine()) != null)
                    {
                        //format id with left zeros
                        id = FormatZeros(line);
                        if (id != "")
                        {
                            //recover data
                            log.Info("Recovering " + id);
                            json = GetAPI(id).Result;

                            //save data as json
                            log.Info("Saving " + id);
                            System.IO.StreamWriter backup = new System.IO.StreamWriter(id + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".json");
                            backup.Write(json);
                            backup.Close();

                            //delete
                            log.Info("Deleting " + id);
                            if (DeleteAPI(id).Result)
                            {
                                log.Info("Deleted " + id);
                            }
                            else
                            {
                                log.Info("Error deleting " + id);
                            }
                            counterOk++;
                        }
                        else
                        {
                            log.Info("Can't process line: " + line);
                            counterNok++;
                        }
                    }
                    file.Close();
                }
                catch (Exception ex)
                {
                    log.Info("Error: ", ex);
                }
                finally
                {
                    log.Info(counterOk.ToString() + " processed lines.");
                    log.Info(counterNok.ToString() + " failed lines.");
                    log.Info("Finished");
                    System.Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: ", ex.Message);
            }
        }

        static async Task<string> GetAPI(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Configuration.BaseUri);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add(Configuration.Headers["ClientIDName"], Configuration.Headers["ClientIDValue"]);
                client.DefaultRequestHeaders.Add(Configuration.Headers["ClientSecretName"], Configuration.Headers["ClientSecretValue"]);

                HttpResponseMessage response = await client.GetAsync(Configuration.RelativeGetUri + "" + id);
                
                string json = "";
                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    //Console.WriteLine("{0}", json);
                }

                return json;
            }
        }

        static async Task<bool> DeleteAPI(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Configuration.BaseUri);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add(Configuration.Headers["ClientIDName"], Configuration.Headers["ClientIDValue"]);
                client.DefaultRequestHeaders.Add(Configuration.Headers["ClientSecretName"], Configuration.Headers["ClientSecretValue"]);

                HttpResponseMessage response = await client.DeleteAsync(Configuration.RelativeDeleteUri + "" + id);

                return response.IsSuccessStatusCode;
            }
        }

        static string FormatZeros(string s)
        {
            double d;
            if (double.TryParse(s, out d))
            {
                return String.Format("{0:00000000000}", d);
            }
            return "";
        }
    }
}