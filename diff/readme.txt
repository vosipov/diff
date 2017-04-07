Assignment notes

Solution was created with Visual Studio 2017 and contains 3 projects
diff - wep api project, service implementation
diff.Integration.Tests - integration tests, MSTest
diff.Tests - unit tests, MSTest

How to use
clone https://github.com/vosipov/diff
Open solution in Visual Studio 2017 and build
Goto diff project properties -> web -> Set start action to [Don't open a page]
Hit F5, IIS Express will start service, copy service url from IIS Express
Use your preffered tool to test API, I was using Postman
Run Integration tests
Run Unit tests

Authentication is disabled because I have no assumptions on its requirements
id type is not defined but sample input/output shows integer examples, so I will use integer type.
System behavior for non Base64 string input(PUT) is not specified. I will return BadRequest.

The input is stored in memory, it is simplest and fastest approach, but has its drawbacks, e.g. panned reboot may result in data loss. Depending on the requirements different IDiffObjectRepository implementations can be provided.

Long strings and memory limits
MaxRequestLength and maxAllowedContentLength should be configured in web.config. Edge case when string exceeds MaxRequestLength is not covered in integrations tests
I am using byte arrays to store input. Maximum array size is 2GB, in practice even smaller, so if the service is configured to accept strings larger than 1GB we can see OutOfMemoryException on large inputs.
I assume that this service is created to compare smaller objects, but if it's not the case we should compile to x64, adjust gcAllowVeryLargeObjects.
With byte array we still can't get past 2Gb because of other array limitations like maximum index number. To overcome this different data structures are required, like our own implementation of BigArray.



