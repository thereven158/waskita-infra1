//using UnityEngine;
//using Agate.Waskita.API;
//using Agate.Waskita.Responses;
//using Agate.Waskita.Request;
//using Agate.Waskita.Request.Data;
//using System.Net;
//using UnityEngine.UI;
//using System.Threading.Tasks;
//using Agate.Waskita.Responses.Data;
//using System.Net.Http;
//using UnityEngine.Networking;
//using System.Collections;
//using System.IO;

//public class ExampleBypass : MonoBehaviour
//{
//    public Button btnValidate, btnSaveJson, btnLoadJson, btnLearning, btnImage, btnOpenMaterial, btnCloseMaterial, btnNewProject, btnNextDay, btnPoints;
//    public Image image;

//    private WaskitaApi api;
//    private MaterialsResponse BSLData;
//    private readonly string dummyUserID = "001901741";
//    private string token;
//    private string deviceID;
//    private int readID = 0;
//    private int projectID = 0;


//    void Awake()
//    {
//        var test = GetComponent<MonoBehaviour>();
//        api = new WaskitaApi(test);
//        deviceID = "aa";

//        #region Button Auth
//        btnValidate.onClick.AddListener(delegate { Validate(); });
//        btnSaveJson.onClick.AddListener(delegate { SaveData(); });
//        btnLoadJson.onClick.AddListener(delegate { LoadData(); });
//        #endregion

//        #region BSL
//        btnLearning.onClick.AddListener(delegate { AllMaterials(); });
//        btnOpenMaterial.onClick.AddListener(delegate { OpenMaterial(); });
//        btnCloseMaterial.onClick.AddListener(delegate { CloseMaterial(); });
//        btnImage.onClick.AddListener(delegate { BSLImage(); });
//        #endregion

//        #region Project
//        btnNewProject.onClick.AddListener(delegate { NewProject(); });
//        btnNextDay.onClick.AddListener(delegate { NextDay(); });
//        btnPoints.onClick.AddListener(delegate { AddPoint(); });
//        #endregion

//        api.SetToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiIwMDE5MDE3NDEiLCJqdGkiOiI2YzdmMzE1Zi0xMWRhLTQ3OTUtODA1NS0zNzQwYjY4MDZkYzIiLCJleHAiOjE1NzcyNTA1MDQsImlzcyI6IldBU0tJVEEuU2VjdXJpdHkuQmVhcmVyIiwiYXVkIjoiV0FTS0lUQS5TZWN1cml0eS5CZWFyZXIifQ.sVB9hTSTiTuuHeMRIV5ZC0BBzDlBIJ43u8wf6G5gTtA");
        
//        api.SetAddress("https://game-waskita-iptex-stag.agatedev.net/");
//    }
//    #region Auth EndPoint

//    /// <summary>
//    /// Validate request berfungsi untuk pengecekan apakah token sudah expired? server sedang online atw tidak?
//    /// Selalu panggil fungsi ini jika user sudah pernah login sebelumnya
//    /// pada dasarnya token akan expired dalam 30 hari, jika request ini return error Unauthorized maka token sudah expired
//    /// </summary>
//    /// <returns></returns>
//    public void Validate()
//    {
//        Debug.Log("Melakukan request validate");
//        BasicRequest request = new BasicRequest
//        {
//            deviceId = deviceID,
//            gameVersion = "1.0",
//            userId = dummyUserID
//        };

//        api.Validate(request, success => {
//            Debug.Log(JsonUtility.ToJson(success));
//            if (success.error == null)
//            {
//                //Debug.Log(success.data.currentProject.currentEnergy);
//                //Debug.Log(success.responseMessage.message);
//            }
//            else
//            {
//                //ini merupakan bad request
//                //Debug.Log(success.responseMessage.message);
//            }
//        }, HandleError);
//    }

//    /// <summary>
//    /// Menyimpan json string dari frontend
//    /// Mungkin untuk kebutuhan settings game?
//    /// </summary>
//    public void SaveData()
//    {
//        Debug.Log("Melakukan request SaveData Json");
//        SaveRequest request = new SaveRequest
//        {
//            deviceId = deviceID,
//            gameVersion = "1.0",
//            jsonString = "{key: \"value\"}", //sesuai requirement frontend, untuk kebutuhan setting json frontend ? 
//            userId = dummyUserID
//        };

//        api.SaveJson(request, success => {
//            Debug.Log(JsonUtility.ToJson(success));
//            //if (success.responseMessage.responseCode == 1001)
//            //{
//            //    Debug.Log(success.responseMessage.message);
//            //}
//            //else
//            //{
//            //    //ini merupakan bad request
//            //    Debug.Log(success.responseMessage.message);
//            //}
//        }, HandleError);
//    }

//    /// <summary>
//    /// Load data json string
//    /// </summary>
//    public void LoadData()
//    {
//        Debug.Log("Melakukan request LoadData Json");
//        BasicRequest request = new BasicRequest
//        {
//            deviceId = deviceID,
//            gameVersion = "1.0",
//            userId = dummyUserID,
//        };

//        api.LoadJson(request, success => {
//            //if (success.responseMessage.responseCode == 1001)
//            //{
//            //    Debug.Log(success.responseMessage.message);
//            //}
//            //else
//            //{
//            //    //ini merupakan bad request
//            //    Debug.Log(success.responseMessage.message);
//            //}
//        }, HandleError);
//    }
//    #endregion

//    #region Materials EndPoint
//    /// <summary>
//    /// Request ini dilakukan sebagai pengecekan jika ada gambar / assets baru untuk kebutuhan BSL
//    /// </summary>
//    /// <returns></returns>
//    public void BSLImage()
//    {
//        Debug.Log("Melakukan request download Image BSL");
//        // list di sisi dengan daftar gambar yang ada di lokal device 
//        //(jika tidak ada sama sekali, berarti list di dalamnya kosong saja / null)
//        System.Collections.Generic.List<ImageDetail> images = new System.Collections.Generic.List<ImageDetail>
//        {
//            new ImageDetail{ imageName = "contoh-gambar-dilokal", version = 1 }
//        }; 
        
//        BSLImageRequest request = new BSLImageRequest
//        {
//            data = images,
//            deviceId = deviceID,
//            gameVersion = "1.0",
//            userId = dummyUserID
//        };
//        api.BSLImage(
//            request,
//            success => {
//                Debug.Log(JsonUtility.ToJson(success));
//                if (success.responseMessage.responseCode == 1001)
//                {
//                    Debug.Log(success.responseMessage.message);
//                    //Implementasi delete & save file di lokal
//                    //Contoh di bawah akan melakukan save di Application.persistentDataPath
//                    //Implementasi bisa di sesuaikan dengan kebutuhan (ini hanya contoh)
//                    DownloadHandler(success.downloadAssets, success.deleteAssets);
//                }
//                else
//                {
//                    //ini merupakan bad request
//                    Debug.Log(success.responseMessage.message);
//                }
//            },
//            HandleError
//        );
//    }

//    /// <summary>
//    /// Request ini dilakukan untuk menampilkan konten BSL
//    /// </summary>
//    /// <returns></returns>
//    public void AllMaterials()
//    {
//        Debug.Log("Melakukan request content BSL");
//        //show all category "Integrity" "Professionalism" "Teamwork" "Excellence"
//        //dikarenakan ENUM di DB return string dan c# return int
//        //kedepannya akan ada sedikit perbaikan untuk hal ini
//        MaterialsRequest request = new MaterialsRequest
//        {
//            category = "", // all category "Integrity" "Profesionalism" "Teamwork" "Excellence"
//            deviceId = deviceID,
//            gameVersion = "1.0",
//            userId = dummyUserID
//        };
//        api.LearningMaterials(
//            request,
//            success => {
//                Debug.Log(JsonUtility.ToJson(success));
//                if (success.responseMessage.responseCode == 1001)
//                {
//                    Debug.Log(success.responseMessage.message);
//                    BSLData = success;
//                    ShowAllData();
//                }
//                else
//                {
//                    //ini merupakan bad request
//                    Debug.Log(success.responseMessage.message);
//                }
//            },
//            HandleError
//        );
//    }

//    /// <summary>
//    /// Request ini dilakukan ketika user memilih salah satu set card
//    /// dimana satu set card dibedakan berdasrkan materiID
//    /// materiID inilah yang dijadikan paramter untuk request ini
//    /// NOTE : request ini untuk melihat seberapa lama user membaca materi di BSL
//    /// </summary>
//    /// <returns></returns>
//    public void OpenMaterial()
//    {
//        Debug.Log("Melakukan request Open Learning Material");
//        ReadRequest request = new ReadRequest
//        {
//            deviceId = deviceID,
//            gameVersion = "1.0",
//            materiId = 24, //materiID ini didapatkan pada materi pembelajaran yang dipilih oleh user, dimana frontend melakukan request 
//            //allmaterials terlebih dahulu. cek class "MaterialLearning"
//            userId = dummyUserID
//        };

//        api.OpenMaterial(request, success => {
//            if (success.responseMessage.responseCode == 1001)
//            {
//                Debug.Log(JsonUtility.ToJson(success));
//                readID = success.data.readMaterialId;
//                Debug.Log("Menyimpan read ID" + readID.ToString());
//                Debug.Log(success.responseMessage.message);
//            }
//            else
//            {
//                //ini merupakan bad request
//                Debug.Log(success.responseMessage.message);
//            }
//        }, HandleError);
//    }

//    /// <summary>
//    /// Request ini dilakukan jika user sudah selesai / menutup set card
//    /// </summary>
//    /// <returns></returns>
//    public void CloseMaterial()
//    {
//        Debug.Log("Melakukan request Close Learning Material");
//        FinishReadRequest request = new FinishReadRequest
//        {
//            deviceId = deviceID,
//            gameVersion = "1.0",
//            readMateriId = readID,
//            userId = dummyUserID
//        };
//        api.CloseMaterial(request, success => {
//            Debug.Log(JsonUtility.ToJson(success));
//            if (success.responseMessage.responseCode == 1001)
//            {
//                Debug.Log(success.responseMessage.message);
//            }
//            else
//            {
//                //Bad request
//                Debug.Log(success.responseMessage.message);
//            }
//        }, HandleError);
//    }
//    #endregion

//    #region Projects

//    /// <summary>
//    /// Request ini dilakukan jika user akan menjalankan project baru
//    /// </summary>
//    /// <returns></returns>
//    public void NewProject()
//    {
//        Debug.Log("Melakukan request NewProject");
//        TakeProjectRequest request = new TakeProjectRequest
//        {
//            deviceId = deviceID,
//            gameVersion = "1.0",
//            level = 1, //Sehubungan dengan pembahasan terkahir bahwa project tidak bisa di pilih, dan berjalan sesuai susunan
//            //maka update selanjutnya, parameter ini akan dihilangkan
//            userId = dummyUserID
//        };
//        api.NewProject(request, success => {
//            if (success.responseMessage.responseCode == 1001)
//            {
//                Debug.Log(JsonUtility.ToJson(success));
//                projectID = success.data.proyekId;
//                Debug.Log("Menyimpan project id " + projectID.ToString());
//            }
//            else
//            {
//                //Bad request
//                Debug.Log(success.responseMessage.message);
//            }
//        }, HandleError);
//    }

//    /// <summary>
//    /// Untuk melakukan update hari keberapakah user saat ini
//    /// selain itu, fungsi ini akan melakukan penambahan pada energy user di DB
//    /// </summary>
//    /// <returns></returns>
//    public void NextDay()
//    {
//        Debug.Log("Melakukan request NextDay");
//        BasicRequest request = new BasicRequest
//        {
//            deviceId = deviceID,
//            gameVersion = "1.0",
//            userId = dummyUserID,
//        };

//        api.CurrentDay(request, success => {
//            Debug.Log(JsonUtility.ToJson(success));
//            if (success.responseMessage.responseCode == 1003)
//            {
//                //sukses update current day, project sudah beres dengan status sukses 
//                Debug.Log(success.responseMessage.message);
//            }
//            else if (success.responseMessage.responseCode == 1004)
//            {
//                //sukses update current day
//                Debug.Log(success.responseMessage.message);
//            }
//            else if (success.responseMessage.responseCode == 1005)
//            {
//                //sukses update current day, project sudah beres dengan status gagal
//                Debug.Log(success.responseMessage.message);
//            }
//            else
//            {
//                //ini badrequest
//                Debug.Log(success.responseMessage.message);
//            }
//        }, HandleError);
//    }

//    /// <summary>
//    /// Untuk menambahkan progress point
//    /// </summary>
//    /// <returns></returns>
//    public void AddPoint()
//    {
//        Debug.Log("Melakukan request AddPoint");
//        AddPointRequest request = new AddPointRequest
//        {
//            npc = "npc id right here",
//            area = 1, //sesuai requirement frontend
//            energy = 10, //berapa banyak energy yang dikurangi untuk menjalankan quiz/event ini
//            type = "Event", //type hanya ada "Event" dan "Quiz"
//            points = new System.Collections.Generic.List<PointData> { 
//                //Points sengaja dijadikan array / list jika memang ada event/quiz
//                //yang bisa mempengaruhi 2 point type sekaligus
//                new PointData {
//                    point = 10,
//                    pointType = "I" //Point type berisikan "I" "P" "T" "Ex"
//                },
//            },
//            gameVersion = "1.0",
//            userId = dummyUserID,
//            deviceId = deviceID,
//        };
//        api.AddPoint(request, success => {
//            Debug.Log(JsonUtility.ToJson(success));
//            if (success.responseMessage.responseCode == 1001)
//            {
//                Debug.Log(success.responseMessage.message);
//            }
//            else
//            {
//                //ini badrequest
//                Debug.Log(success.responseMessage.message);
//            }
//        }, HandleError);
//    }

//    #endregion

//    #region All Helper Function
//    /// <summary>
//    /// handle error
//    /// </summary>
//    /// <param name="error"></param>
//    public void HandleError(BasicResponse error)
//    {
//        Debug.Log(error.responseMessage.message);
//        Debug.Log(error.responseMessage.responseCode);
//    }
//    public void ShowAllData()
//    {
//        foreach (MaterialLearning data in BSLData.data)
//        {
//            Debug.Log(data.category);
//            Debug.Log(data.session);
//            foreach (Card card in data.contents)
//            {
//                Debug.Log(card.panelType);
//                Debug.Log(card.imageName); // untuk image bsa di set gambar apa aja dlu, krn blm fix apakah file gambar
//                //backend yang menentukan atau backend hanya memberikan nama gambar dan frontend yang menentukan yang mana.
//                Debug.Log(card.author);
//                Debug.Log(card.title);
//                Debug.Log(card.subTitle);
//                Debug.Log(card.content);
//                foreach (Collapsible collaps in card.collapsibles)
//                {
//                    Debug.Log("Collaps title " + collaps.title);
//                    Debug.Log("Collaps content " + collaps.content);
//                }
//            }
//        }
//    }
//    public void DownloadHandler(System.Collections.Generic.List<BSLImage> images, System.Collections.Generic.List<string> delete)
//    {
//        foreach(string del in delete)
//        {
//            File.Delete(Application.persistentDataPath + "/" + del + ".png");
//        }
//        foreach(BSLImage data in images)
//        {
//            DoDownload(data.url, data.imageName);
//        }
//    }
//    private void DoDownload(string url, string imageName)
//    {
//        StartCoroutine(LoadSpriteIMG(url, imageName));
//    }
//    IEnumerator LoadSpriteIMG(string imageURL, string imageName)
//    {
//        if (Application.internetReachability == NetworkReachability.NotReachable)
//            yield return null;

//        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
//        yield return www.SendWebRequest();

//        if (www.isNetworkError || www.isHttpError)
//        {
//            Debug.Log(www.error);
//        }
//        else
//        {
//            Debug.Log(imageURL);
//            Debug.Log(imageName);
//            Debug.Log("download image");
//            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture as Texture2D;
//            Sprite sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), Vector2.zero);

//            image.sprite = sprite;
//            byte[] bytes = myTexture.EncodeToPNG();
//            Debug.Log(Application.persistentDataPath + "/" + imageName + ".png");
//            File.WriteAllBytes(Application.persistentDataPath + "/" + imageName + ".png", bytes);
//        }
//    }
//    #endregion
//}
