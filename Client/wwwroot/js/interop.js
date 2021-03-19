var StoryForce = StoryForce || {};

StoryForce.Interop = {
    uploadFile: function(postUrl, file, description) {
        const MaxFileSizeMB = 1;
        const BufferChunkSize = MaxFileSizeMB * (1024 * 1024);

        //var progressinfo = document.getElementById(id + 'ProgressInfo');
        //var progressbar = document.getElementById(id + 'ProgressBar');

        let uploadedFile = null;

        const fileChunk = [];
        let fileStreamPos = 0;
        let endPos = BufferChunkSize;

        /*        progressbar.setAttribute("style", "visibility: visible;");*/

        while (fileStreamPos < file.size) {
            fileChunk.push(file.slice(fileStreamPos, endPos));
            fileStreamPos = endPos;
            endPos = fileStreamPos + BufferChunkSize;
        }

        var totalParts = fileChunk.length;
        var partCount = 0;

        let chunk;
        while (chunk = fileChunk.shift()) {
            partCount++;
            var fileName = file.name +
                ".part_" +
                partCount.toString().padStart(4, "0") +
                "_" +
                totalParts.toString().padStart(4, "0");

            var data = new FormData();
            data.append("file", chunk, fileName);
            data.append("description", description);

            var request = new XMLHttpRequest();
            request.responseType = "json";
            request.open("POST", postUrl, true);

            request.upload.onloadstart = function(e) {
                //progressbar.value = 0;
                //progressinfo.innerHTML = file.name + ' 0%';
            };

            request.upload.onprogress = function(e) {
                var percent = Math.ceil((e.loaded / e.total) * 100);
                //progressbar.value = (percent / 100);
                //progressinfo.innerHTML = file.name + '[' + partCount + '] ' + percent + '%';
            };

            request.upload.onloadend = function(e) {
                //progressbar.value = 1;
                //progressinfo.innerHTML = file.name + ' 100%';
                uploadedFile = request.response.uploadedFile;
            };

            request.send(data);
        }

        return uploadedFile;
    },
    requestUpload: function (postUrl, fileName, formData, chunkIndex) {
        return new Promise(function(resolve, reject) {
            const xhr = new XMLHttpRequest();
            xhr.responseType = "json";
            xhr.open("POST", postUrl, true);

            xhr.upload.onloadstart = function(e) {
                console.log(fileName + " 0%");
            };

            xhr.upload.onprogress = function(e) {
                var percent = Math.ceil((e.loaded / e.total) * 100);
                console.log(fileName + "[" + chunkIndex + "] " + percent + "%");
            };

            xhr.upload.onloadend = function(e) {
                console.log(fileName + "[" + chunkIndex + "] " + " 100%");
            };

            xhr.onreadystatechange = function() {
                if (xhr.readyState === XMLHttpRequest.DONE) {
                    const status = xhr.status;
                    if (status === 0 || (status >= 200 && status < 400)) {
                        // The request has been completed successfully
                        console.log(xhr.response);
                        resolve(xhr.response);
                    } else {
                        // Oh no! There has been an error with the request!
                        reject(xhr.response);
                    }
                } 
            }

            xhr.send(formData);
        });
    },
    uploadFiles: async function(postUrl, fileInputId, descriptions) {
        const MaxFileSizeMB = 10;
        const BufferChunkSize = MaxFileSizeMB * (1024 * 1024);
        const files = document.getElementById(fileInputId).files;
        const currentTime = Date.now() - new Date().getTimezoneOffset() * 60 * 1000;
        const uploadedFiles = [];

        for (let i = 0; i < files.length; i++) {
            const fileChunk = [];
            const file = files[i];
            const description = descriptions[i];
            let fileStreamPos = 0;
            let endPos = BufferChunkSize;

            while (fileStreamPos < file.size) {
                fileChunk.push(file.slice(fileStreamPos, endPos));
                fileStreamPos = endPos;
                endPos = fileStreamPos + BufferChunkSize;
            }

            const totalParts = fileChunk.length;
            const fileName = `${currentTime}-${file.name}`;
            uploadedFiles.push(fileName);

            for (let cIdx = 0; cIdx < fileChunk.length; cIdx++) {
                const chunk = fileChunk[cIdx];

                if (cIdx > totalParts - 1) {
                    return;
                }

                const data = new FormData();
                data.append("file", chunk, fileName);
                data.append("description", description);
                data.append("totalSize", file.size);

                await this.requestUpload(postUrl, fileName, data, cIdx + 1);
            }

            return uploadedFiles;
        };
    },
    uploadFileByUrl: function (postUrl, fileUrl, fileName, description) {
        var data = new FormData();
        data.append("url", fileUrl);
        data.append("fileName", fileName);
        data.append("description", description);

        var request = new XMLHttpRequest();
        request.open("POST", postUrl, true);

        request.upload.onloadstart = function (e) {
            progressbar.value = 0;
            progressinfo.innerHTML = file.name + " 0%";
        };

        request.upload.onprogress = function (e) {
            var percent = Math.ceil((e.loaded / e.total) * 100);
            progressbar.value = (percent / 100);
            progressinfo.innerHTML = file.name + " " + percent + "%";
        };

        request.upload.onloadend = function (e) {
            progressbar.value = 1;
            progressinfo.innerHTML = file.name + " 100%";
        };

        request.send(data);

    },
    getHTML: function(url) {
        return new Promise(function (resolve, reject) {
            var xhr = new XMLHttpRequest();
            xhr.open('get', url, true);
            xhr.responseType = 'document';
            xhr.onload = function () {
                var status = xhr.status;
                if (status == 200) {
                    resolve(xhr.response.documentElement.innerHTML);
                } else {
                    reject(status);
                }
            };
            xhr.send();
        });
    },
    schemaPageHandler: async function() {
        try {
            var parser = new window.DOMParser();
            var remoteCode = await this.getHTML('https://schema.org/docs/full.html');
            var sourceDoc = parser.parseFromString(remoteCode, 'text/html');
            var thingList = sourceDoc.getElementById("C.Thing");
            var el = document.createElement("div");
            el.id = "structured-data-types";
            el.appendChild(thingList);
            document.body.appendChild(el);

            await new Promise(r => setTimeout(r, 5000));
            return "Done";
        } catch (error) {
            console.log("Error fetching remote HTML: ", error);
        }
    }
}