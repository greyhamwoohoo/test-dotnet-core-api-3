# test-dotnet-core-api-3
.Net Core 3.1 REST API; sample stuff I often end up using for testing. 

## Integration Tests
Integration Tests use the Microsoft 'TestServer' implementation (in memory) and HttpClient.

| Reference | Link |
| --------- | ---- |
| Integration Tests in .Net Core 3.0 | https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1 |
| Converting Integration Tests from 2.0x to 3.0 | https://andrewlock.net/converting-integration-tests-to-net-core-3/ |


### Contract Tests
A crude (but effective) way to lock in the shapes of your contracts. 

Works well enough if you know your consumers and are building to their use cases; not a bad starting point. Piggy backs on your functional testing framework. 

| Reference | Link |
| --------- | ---- |
| Consumer Contract Driven Testing: for a larger scale contract testing strategy | https://pact.io |

## Acceptance Tests
Self-hosts the service using a 'hard' port binding (does not use TestServer in its startup)

Shows Serilog registration and integration; scoped lifetimes; builders. 

### Test Execution Context
To allow the same test to target different environments or URLs, we need to separate the endpoints, URL's, DI configuration/injection, certificates, authorization tokens and so forth from the test itself. 

These variations are stored in a 'Test Execution Context' and injected into the Test Runner at execution time. Test Execution Contexts are stored as .json files in the TestExecutionContexts folder: Json is preferred because it is easier to read and write complex structures. AcceptanceTests\InitializeTestRun.cs shows the code to do this. 

Visual Studio has no concept of a test execution context: but it does have the concept of 'RunSettings'. A .runsettings file is included for each Test Execution Context and can be chosen from the Visual Studio from the Test / Configure Run Settings / Select Solution Wide runsettings File menu option. The code in InitializeTestRun.cs will read the TestExecutionContext from the .runsettings file and ensure the correct context - variables and so forth - are injected. 

The 'InProcess' setting will cause the API to be 'self-hosted': this allows us to override the DI with custom implementations as required and debug from the test through to the application code directly. 

| Reference | Link |
| --------- | ---- |
| Configure unit/integration/acceptance tests with a .runsettings file | https://docs.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2019 |

## Builders / TestSdk
A 'Test SDK' is included with some bootstrapped builders. Builders are a fluent way of constructing payloads ('convention over configuration'). 

## Logging
Logging is included using SeriLog both in the ASP.Net Core API and in the test framework itself.

| Reference | Link |
| --------- | ---- |
| Serilog in .Net Core 3.0 | https://nblumhardt.com/2019/10/serilog-in-aspnetcore-3/ |
| Serilog Configuration Inline | https://github.com/serilog/serilog-aspnetcore |
| Serilog Json Configuration | https://github.com/serilog/serilog-settings-configuration |

## References
