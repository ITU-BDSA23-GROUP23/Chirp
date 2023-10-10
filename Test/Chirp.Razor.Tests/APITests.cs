namespace Chirp.Razor.Tests;
using Xunit;
using System.Net;
public class APITests
{

    //Credit and see more: https://zetcode.com/csharp/getpostrequest/?utm_content=cmp-true
    [Fact]
    public void HtmlVerificationForEndpointsPublic()
    {
        // Arrange
        var url = "http://localhost:5273/?page=21"; //public timeline
        var request = WebRequest.Create(url);
        request.Method = "GET";

        // Act
        using var webResponse = request.GetResponse(); //returns a web response containing the response to the request.
        using var webStream = webResponse.GetResponseStream(); //gets the instance of the stream class for reading data

        using var reader = new StreamReader(webStream);
        var HttpResponseBody = reader.ReadToEnd(); //We read all the data from the stream.
        

        // Assert
        Assert.Contains("Hello, BDSA students!", HttpResponseBody);
        //Console.WriteLine(HttpResponseBody);
    }

    [Fact]
    public void HtmlVerificationForEndpointsPrivate()
    {
        // Arrange
        var url = "http://localhost:5273/Rasmus"; //private timeline, may need to be /Rasmus later.
        var request = WebRequest.Create(url);
        request.Method = "GET";

        // Act
        using var webResponse = request.GetResponse(); //returns a web response containing the response to the request.
        using var webStream = webResponse.GetResponseStream(); //gets the instance of the stream class for reading data

        using var reader = new StreamReader(webStream);
        var HttpResponseBody = reader.ReadToEnd(); //We read all the data from the stream.

        // Assert
        Assert.Contains("Hej, velkommen til kurset.", HttpResponseBody);
        //Console.WriteLine(HttpResponseBody);
    }

    [Fact]
    public void diffrentPageNotSameTest()
    {
         // Arrange
        var url = "http://localhost:5273/?page=1"; //public timeline
        var request = WebRequest.Create(url);
        request.Method = "GET";

        var url2 = "http://localhost:5273/?page=2"; //public timeline
        var request2 = WebRequest.Create(url2);
        request2.Method = "GET";


        // Act
        using var webResponse = request.GetResponse(); //returns a web response containing the response to the request.
        using var webStream = webResponse.GetResponseStream(); //gets the instance of the stream class for reading data

        using var reader = new StreamReader(webStream);
        var HttpResponseBody = reader.ReadToEnd(); //We read all the data from the stream.

        using var webResponse2 = request2.GetResponse(); //returns a web response containing the response to the request.
        using var webStream2 = webResponse2.GetResponseStream(); //gets the instance of the stream class for reading data

        using var reader2 = new StreamReader(webStream2);
        var HttpResponseBody2 = reader2.ReadToEnd(); //We read all the data from the stream.

        Assert.NotEqual(HttpResponseBody2, HttpResponseBody);

    }

    [Fact]
    public void defaultPageSameAsPage1() 
    {

          // Arrange
        var url = "http://localhost:5273/?page=1"; //public timeline
        var request = WebRequest.Create(url);
        request.Method = "GET";

        var url2 = "http://localhost:5273/"; //public timeline
        var request2 = WebRequest.Create(url2);
        request2.Method = "GET";


        // Act
        using var webResponse = request.GetResponse(); //returns a web response containing the response to the request.
        using var webStream = webResponse.GetResponseStream(); //gets the instance of the stream class for reading data

        using var reader = new StreamReader(webStream);
        var HttpResponseBody = reader.ReadToEnd(); //We read all the data from the stream.

        using var webResponse2 = request2.GetResponse(); //returns a web response containing the response to the request.
        using var webStream2 = webResponse2.GetResponseStream(); //gets the instance of the stream class for reading data

        using var reader2 = new StreamReader(webStream2);
        var HttpResponseBody2 = reader2.ReadToEnd(); //We read all the data from the stream.

        Assert.Equal(HttpResponseBody2, HttpResponseBody);




    }

}