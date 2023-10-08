# Option Patterns Using a Minimal API

This is a .NET 7 minimal API project that tests the lifetimes and reload capabilities of IOptions, IOptionsSnapshot, and IOptionsMonitor. Microsoft has created a great document titled [Options pattern in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/options) to explain these concepts, but I wanted to experiment with it through code.

In summary, IOptions provides no reload capabilities. Once the configurations are registered during start-up, any changes to a configuration will not take effect within the application. On the other hand, IOptionsSnapshot and IOptionsMonitor have reload capabilities, and changes to a configuration will eventually be portrayed within the application. The main difference between IOptionsSnapshot and IOptionsMonitor is their lifetimes.

To set the scene, assume we have a live application experiencing issues due to a newly deployed feature. We have diagnosed the issue and have determined the feature can be disabled by changing a value within our configuration file, `appsettings.json`. With the Options pattern, and depending on the interface used, we must determine if we will need to restart our application. 

The best explanation of how each interface works can be found on Stack Overflow:
> Use IOptions<T> when you are not expecting your config values to change. Use IOptionsSnaphot<T> when you are expecting your values to change but want it to be consistent for the entirety of a request. Use IOptionsMonitor<T> when you need real time values [, i.e., up-to-date values on the next injection of IOptionsMonitor].
>
>  &mdash; <cite>[Stack Overflow](https://stackoverflow.com/a/61929399)</cite>

What separates IOptionsSnapshot and IOptionsMonitor, and the main reason to choose one over the other, would be their lifetimes. IOptionsSnapshot is designed for use with transient and scoped dependencies, while IOptionsMonitor is useful in singleton dependencies. In other words, if you have a singleton service and you need to access Options with reload capabilities, then your only option is IOptionsMonitor. Otherwise, use IOptionsSnapshot.

## Observing Live Configuration Changes Take Effect

To launch this application, we will need to install the [.Net 7 sdk](
https://dotnet.microsoft.com/en-us/download/dotnet/7.0).

Once we have .Net 7, we can start the application by running

```bash
dotnet watch run --project .\src\OptionPatternsWithValidation\OptionPatternsWithValidation.csproj
```

, or by pressing F5 if you are using VS2022 or VSCode.

The application will display a Swagger UI that can be used to test each endpoint. When executing each endpoint, please make note of the response body. The response body will contain an `ApiKey` value that will initially read `Test_0`.

Now, navigate to the `appsettings.json` file and change `AppOptions:ApiKey` to any value, such as `Test_1`, and save the file.

After executing each endpoint again, we will see that the `/optionsSnapshot` and `/optionsMonitor` endpoints both reflect this new value, while `/options` continues to display `Test_0`.

This is the power of `/optionsSnapshot` and `/optionsMonitor`. `/options` is also useful. It can prevent accidental changes to a configuration from taking effect.