using VertexHRMS.DAL.Entities;
using FaceRecognitionDotNet;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IFaceRecognitionService
    {
        FaceEncoding? EncodeImage(string imagePath);
        Task<Employee?> Recognize(string imagePath, double tolerance = 0.6);
        void Dispose();
    }
}
