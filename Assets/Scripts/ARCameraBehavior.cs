using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARCameraBehavior : MonoBehaviour
{
    [SerializeField]
    ARCameraManager cameraManager;
    Texture2D m_Texture;
    private UnityMessageManager Manager {
        get {return GetComponent<UnityMessageManager>(); }
        // Manager.SendMessageToFlutter(Convert.ToBase64String(fileData));
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public unsafe void sendImage(string args) {
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            return;

        var conversionParams = new XRCpuImage.ConversionParams
        {
            // Get the entire image.
            inputRect = new RectInt(0, 0, image.width, image.height),

            // Downsample by 2.
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),

            // Choose RGBA format.
            outputFormat = TextureFormat.RGBA32,

            // Flip across the vertical axis (mirror image).
            transformation = XRCpuImage.Transformation.MirrorY
        };

        // See how many bytes you need to store the final image.
        int size = image.GetConvertedDataSize(conversionParams);

        // Allocate a buffer to store the image.
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        // Extract the image data
        image.Convert(conversionParams, new IntPtr(buffer.GetUnsafePtr()), buffer.Length);

        // The image was converted to RGBA32 format and written into the provided buffer
        // so you can dispose of the XRCpuImage. You must do this or it will leak resources.
        image.Dispose();

        // At this point, you can process the image, pass it to a computer vision algorithm, etc.
        // In this example, you apply it to a texture to visualize it.

        // You've got the data; let's put it into a texture so you can visualize it.
        m_Texture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat,
            false);

        m_Texture.LoadRawTextureData(buffer);
        m_Texture.Apply();
        Manager.SendMessageToFlutter(Convert.ToBase64String(m_Texture.EncodeToJPG()));

        // Done with your temporary data, so you can dispose it.
        buffer.Dispose();
    }

    void showInfo(string args) {

    }
}
