using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace Company.Custom
{

    class FacialRecognition
    {
        [FunctionName("FacialRecognition")]
        public static async Task<IActionResult>  Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest
            req, ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

        // URL for the images.
        //const string IMAGE_BASE_URL = "https://static.standard.co.uk/2021/08/31/20/newFile.jpg?width=968&auto=webp&quality=75&crop=968%3A645%2Csmart";

        // <snippet_creds>
        // From your Face subscription in the Azure portal, get your subscription key and endpoint.
            string SUBSCRIPTION_KEY = "5f482c987e7e4396acf27b904a315a63";
            const string ENDPOINT = "https://facialreognitionhbla.cognitiveservices.azure.com/";
        // </snippet_creds>

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string imageUrl = data?.imageUrl;

            const string RECOGNITION_MODEL4 = RecognitionModel.Recognition04;

            IFaceClient client = Authenticate(ENDPOINT, SUBSCRIPTION_KEY);

           DetectFaceExtract(client, imageUrl, RECOGNITION_MODEL4).Wait();

            return new OkObjectResult(RecognitionModel.Recognition04);
        }

       public static IFaceClient Authenticate(string endpoint, string key)
        {
        return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        // <snippet_detect>
        /* 
		 * DETECT FACES
		 * Detects features from faces and IDs them.
		 */
        public static async Task DetectFaceExtract(IFaceClient client, string imageUrl, string recognitionModel)
        {

            // Create a list of images
            List<string> imageFileNames = new List<string>
                            {
                                "detection1.jpg",    // single female with glasses
								// "detection2.jpg", // (optional: single man)
								// "detection3.jpg", // (optional: single male construction worker)
								// "detection4.jpg", // (optional: 3 people at cafe, 1 is blurred)
								"detection5.jpg",    // family, woman child man
								"detection6.jpg"     // elderly couple, male female
							};

            foreach (var imageFileName in imageFileNames)
            {
                IList<DetectedFace> detectedFaces;

                // Detect faces with all attributes from image url.
                detectedFaces = await client.Face.DetectWithUrlAsync(imageUrl,
                        returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Accessories, FaceAttributeType.Age,
                        FaceAttributeType.Blur, FaceAttributeType.Emotion, FaceAttributeType.Exposure, FaceAttributeType.FacialHair,
                        FaceAttributeType.Gender, FaceAttributeType.Glasses, FaceAttributeType.Hair, FaceAttributeType.HeadPose,
                        FaceAttributeType.Makeup, FaceAttributeType.Noise, FaceAttributeType.Occlusion, FaceAttributeType.Smile },
                        // We specify detection model 1 because we are retrieving attributes.
                        detectionModel: DetectionModel.Detection01,
                        recognitionModel: recognitionModel);


            }
        }
    }
}    
