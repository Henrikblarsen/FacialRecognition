const uri = 'https://lemon-sand-0a6607a03.azurestaticapps.net/api/FacialRecognition';

function FacialRecognition() {   
    var imageUrl = document.getElementById('imageUrlInput').value;
    var isValidUrl = validateUrl(imageUrl);

    if (isValidUrl == false) {
        document.getElementById('imageDescription').innerHTML = 'Du har ikke angivet en valid url';
        return;
    }

    const jsonBodyItem = {
        imageUrl: imageUrl
    };

    fetch(uri,
        {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(jsonBodyItem)
        })
        .then(response => {
            return response.json()
        })
        .then(data => {
            var imageDiv = document.getElementById('previewImageContainer');
            imageDiv.innerHTML = "";

            var imgTag = document.createElement('img');
            imgTag.src = imageUrl;
            imgTag.classList = 'img-fluid';

            imageDiv.appendChild(imgTag);

            var fullTextResponse = '<h4>Analyze result</h4>';

            fullTextResponse += '<p><b>Description</b>: ' + FaceAttributeType.Age[0].text + '.<p/> ';
            
  
            if (FaceAttributeType.Age == false) {
                fullTextResponse += '<b>The age is </b><br />';
            }
            else {
                fullTextResponse += 'The age cannot be determined<br />';
            }


            fullTextResponse += '<h4>Tags</h4>';

            data.tags.forEach(function (arrayTag) {
                fullTextResponse += 'I am ' + Math.round((arrayTag.confidence * 100 + Number.EPSILON) * 100) / 100 + ' % sure of ' + arrayTag.name + '<br />';
            });


            document.getElementById('imageDescription').innerHTML = fullTextResponse;

            console.log(data)
        })
        .catch(err => {
            document.getElementById('imageDescription').innerHTML = "Something went wrong";
        })
}

function validateUrl(str) {
    var pattern = new RegExp('^(https?:\\/\\/)?' + // protocol
        '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' + // domain name
        '((\\d{1,3}\\.){3}\\d{1,3}))' + // OR ip (v4) address
        '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // port and path
        '(\\?[;&a-z\\d%_.~+=-]*)?' + // query string
        '(\\#[-a-z\\d_]*)?$', 'i'); // fragment locator
    return !!pattern.test(str);
}