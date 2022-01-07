// See https://aka.ms/new-console-template for more information
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Drawing.Text;
using System.Net.Http.Headers;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using CallB2CFunctionShared;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings-local.json", true, true)
    .Build();
var appSettings = config.GetSection("azure").Get<Settings>();

var tenant = appSettings.Tenant;
var azureADB2CHostname = appSettings.Authority;
var clientID = appSettings.ClientID;
var policySignUpSignIn = appSettings.PolicySignUpSignIn;
var authorityBase = $"https://{azureADB2CHostname}/tfp/{tenant}/";
var authority = $"{authorityBase}{policySignUpSignIn}";
var scopes = new string[0];
if (appSettings is not null && appSettings.Scopes is not null)
    scopes = appSettings.Scopes.Split(" ");


var application = PublicClientApplicationBuilder.Create(clientID)
               .WithRedirectUri("http://localhost:12345")
               .WithB2CAuthority(authority)
               .Build();

AuthenticationResult authResult = null;
IEnumerable<IAccount> accounts = await application.GetAccountsAsync(policySignUpSignIn);
IAccount account = accounts.FirstOrDefault();

Console.WriteLine("Opening browser to do authentication");

authResult = await application.AcquireTokenInteractive(scopes)
                        .WithAccount(account)
                        .ExecuteAsync();

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
        if(key == "x")
        {
            stop = true;
        }
    }while(!stop);

}
else
{
    Console.WriteLine("Authentication Failure");
}

Console.ReadLine();


