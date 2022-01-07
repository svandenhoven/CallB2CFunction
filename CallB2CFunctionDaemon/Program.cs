// See https://aka.ms/new-console-template for more information
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using CallB2CFunctionShared;


IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings-local.json", true, true)
    .Build();
var appSettings = config.GetSection("Azure").Get<Settings>();

var clientId = appSettings.ClientID;
var clientSecret = appSettings.ClientSecret;
var authority = new Uri("http://x");
var scopes = new string[0];

if (appSettings.Authority != null)
{
    authority = new Uri(appSettings.Authority);
}
else
{
    Console.WriteLine("Error: Authority is null");
    return;
}


if (appSettings is not null && appSettings.Scopes is not null)
    scopes = appSettings.Scopes.Split(" ");

var app = ConfidentialClientApplicationBuilder.Create(clientId)
    .WithClientSecret(clientSecret)
    .WithAuthority(authority)
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