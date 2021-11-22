using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;


namespace Company.Custom
{
    class FacialRecognition
    {

        // URL for the images.
        const string IMAGE_BASE_URL = "https://static.standard.co.uk/2021/08/31/20/newFile.jpg?width=968&auto=webp&quality=75&crop=968%3A645%2Csmart";

        // <snippet_creds>
        // From your Face subscription in the Azure portal, get your subscription key and endpoint.
        const string SUBSCRIPTION_KEY = "e8abd93bfca34bb9984d4985b0fc7cf2";
        const string ENDPOINT = "https://appwithfacialrecognition.cognitiveservices.azure.com/";
        // </snippet_creds>

        static void Main(string[] args)
        {

            // <snippet_detect_models>
            // Recognition model 4 was released in 2021 February.
            // It is recommended since its accuracy is improved
            // on faces wearing masks compared with model 3,
            // and its overall accuracy is improved compared
            // with models 1 and 2.
            const string RECOGNITION_MODEL4 = RecognitionModel.Recognition04;
            // </snippet_detect_models>

			// <snippet_maincalls>
            // Authenticate.
            IFaceClient client = Authenticate(ENDPOINT, SUBSCRIPTION_KEY);
            // </snippet_client>

            // Detect - get features from faces.
            DetectFaceExtract(client, IMAGE_BASE_URL, RECOGNITION_MODEL4).Wait();
        }

        // <snippet_auth>
        /*
		 *	AUTHENTICATE
		 *	Uses subscription key and region to create a client.
		 */
        public static IFaceClient Authenticate(string endpoint, string key)
        {
        return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        // <snippet_detect>
        /* 
		 * DETECT FACES
		 * Detects features from faces and IDs them.
		 */
        public static async Task DetectFaceExtract(IFaceClient client, string url, string recognitionModel)
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
                detectedFaces = await client.Face.DetectWithUrlAsync($"{url}{imageFileName}",
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
