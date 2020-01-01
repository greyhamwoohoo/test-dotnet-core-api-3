# test-dotnet-core-api-3
Bare-bones .Net Core 3.1 API with boot strapped minimalistic test projects... a reference, because I get tired of implementing this and hunting for snippets again and again :-)

## Example
The API service consists of a few controllers (authorized, anonymous, implicit) and an IProduct/IProductRepository implementation that can be overridden in the Acceptance Tests mock. 

YAML Pipeline will run all categories of tests (from projects using dotnet test; and DLLs using dotnet vstest) and publish separate results for each. 

YAML Pipeline will build a container, start it and run Acceptance Tests against the API hosted in the container. 

## Build Pipelines
There are three YAML Builds included under the azure-devops folder:

| Build | Status | Information |
| ----- | ------ | ----------- |
| pr.yml | N/A | Used for PR Builds. Runs the Unit, Integration and Acceptance Tests from the .csproj files using 'dotnet test' |
| release-vstest.yml | [![Build Status](https://greyhamwoohoo.visualstudio.com/Public-Automation-Examples/_apis/build/status/Release-VsTest-Windows?branchName=master)](https://greyhamwoohoo.visualstudio.com/Public-Automation-Examples/_build/latest?definitionId=20&branchName=master) | Windows: Used for Release Builds. Same as 'pr.yml' but also runs the tests from DLLs using the Visual Studio Test Runner |
| release-dotnet-vstest.yml | [![Build Status](https://greyhamwoohoo.visualstudio.com/Public-Automation-Examples/_apis/build/status/Release-DotNet-VsTest-Linux?branchName=master)](https://greyhamwoohoo.visualstudio.com/Public-Automation-Examples/_build/latest?definitionId=21&branchName=master) | Linux: Used for Release Builds. Runs tests and publishes results from all projects and DLLs using dotnet test and vstest (on Linux); builds and runs tests *AGAINST* the Container |

## Docker
The CI Docker Build will create a :candidate tagged container; and then wait for it to start; then run the Acceptance Tests against the docker image. 

| Reference | Link |
| --------- | ---- |
| Microsoft references for building and running .Net Core Containers in Production | https://github.com/dotnet/dotnet-docker/tree/master/samples |
| Diagnostic Services for APIs | http://geekswithblogs.net/EltonStoneman/archive/2011/12/12/the-value-of-a-diagnostics-service.aspx |

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
| A decent sized sample implementation | https://github.com/andreschaffer/microservices-testing-examples |

## Acceptance Tests
Self-hosts the service using a 'hard' port binding (does not use TestServer in its startup). Essentially: an in-memory Component Test. 

Shows Serilog registration and integration; scoped lifetimes; builders. 

### Test Execution Context
To allow the same test to target different environments or URLs, we need to separate the endpoints, URL's, DI configuration/injection, certificates, authorization tokens and so forth from the test itself. 

These variations are stored in a 'Test Execution Context' and injected into the Test Runner at execution time. Test Execution Contexts are stored as .json files in the TestExecutionContexts folder: Json is preferred because it is easier to read and write complex structures. AcceptanceTests\InitializeTestRun.cs shows the code to do this. 

Visual Studio has no concept of a test execution context: but it does have the concept of 'RunSettings'. A .runsettings file is included for each Test Execution Context and can be chosen from the Visual Studio from the Test / Configure Run Settings / Select Solution Wide runsettings File menu option. The code in InitializeTestRun.cs will read the TestExecutionContext from the .runsettings file and ensure the correct context - variables and so forth - are injected. 

The 'InProcess' setting will cause the API to be 'self-hosted': this allows us to override the DI with custom implementations as required and debug from the test through to the application code directly. 

| Reference | Link |
| --------- | ---- |
| Configure unit/integration/acceptance tests with a .runsettings file | https://docs.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2019 |

## Security Tests
Rationale: Security is always on unless controllers or methods are explicitly opted out. To prevent accidental security blunders, controllers and methods that are not authorized or anonymous must be explicitly acknowledged in the unit tests. 

Unit Tests show how to ensure that all Controllers have an [Authorize] attribute or that no HTTP methods have an [AllowAnonymous] attribute - unless they are explicitly acknowledged (excluded) in the test. A quick and dirty way of stopping accidental checkins where developers have removed Authorization attributes to aid local development. 

The Acceptance Tests show (via MiddlewareSecurityTests) how to ensure that the Middleware Authentication/Authorization is enabled by default. This test in particular is very specific to how you configure your authentication/authorization. 

| Reference | Link |
| --------- | ---- |
| Set up any Authentication to get the tests working | https://docs.microsoft.com/en-us/aspnet/core/security/authentication/windowsauth?view=aspnetcore-3.1&tabs=visual-studio |
| Authorization Workshop | https://github.com/blowdart/AspNetAuthorizationWorkshop |

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
