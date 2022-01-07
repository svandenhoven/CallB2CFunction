// See https://aka.ms/new-console-template for more information
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Text;

var clientId = "<your clientid>";
var clientSecret = "secret";
var Authority = "https://login.microsoftonline.com/<your tenant>.onmicrosoft.com";
var scopes = new string[] { "<your scopes>" };

var app = ConfidentialClientApplicationBuilder.Create(clientId)
    .WithClientSecret(clientSecret)
    .WithAuthority(Authority)
    .Build();

var authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();

if(authResult != null)
{
    Console.WriteLine("Obtained AccessToken.");
    Console.WriteLine("Calling Function.");

    HttpClient client = new HttpClient();
    client.BaseAddress = new Uri("https://b2cauthfunction01.azurewebsites.net/api/B2C_Function?code=KuDenbMfpPw3iXzL2MW0Orjki40eoZntaeEqpagBUNO8tACor2zggw==");
    client.DefaultRequestHeaders.Accept.Clear(); client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);

    var content = new StringContent("{\"name\":\"H10\"}", Encoding.UTF8, "application/json");

    var stop = false;

    do
    {
        var result = await client.PostAsync(client.BaseAddress, content);

        if (result.IsSuccessStatusCode)
        {
            Console.WriteLine(await result.Content.ReadAsStringAsync());
        }

        Console.WriteLine("press a key and enter");
        var key = Console.ReadLine();
        if (key == "x")
        {
            stop = true;
        }
    } while (!stop);
}