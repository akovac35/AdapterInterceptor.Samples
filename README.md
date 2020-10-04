# AdapterInterceptor.Samples

This project contains samples for the [AdapterInterceptor](https://github.com/akovac35/AdapterInterceptor) library. **You should get familiar with it first to understand the samples**.

![this](Resources/.NET_Core_Logo_small.png)

## Executing code
Navigate to ../WebApp and execute ```dotnet run serilog``` or ```dotnet run nlog```. Execute ```dotnet test``` for the TestApp.

## Contents

The following samples are provided:
 * A sample of adapting the ```BlogService``` to use the ```BlogDto``` instead of the original ```Blog``` data transfer object,
 * a sample of using ```ProxyImitatorInterceptor```, which is a variant of ```AdapterInterceptor```, to proxy non-virtual methods.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[Apache-2.0](LICENSE)