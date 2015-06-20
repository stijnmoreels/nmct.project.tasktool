using nmct.project.tasktool.models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models
{
    public class StatusCode
    {
        public enum StatusCodeMessages
        {
            [StringValue("Deleted Succesfully")]
            DELETED_SUCCESS,
            [StringValue("Not Deleted")]
            DELETED_ERROR,
            [StringValue("No ID Specified")]
            NO_ID,
            [StringValue("Added Succesfully")]
            ADD_SUCCESS,
            [StringValue("Not Added")]
            ADD_ERROR,
            [StringValue("Move Card Succesfully")]
            MOVECARD_SUCCESS,
            [StringValue("Not Move Card ")]
            MOVECARD_ERROR,
            [StringValue("Image Uploaded Succesfully ")]
            IMAGE_UPLOAD_SUCCESS,
            [StringValue(" not Image Uploaded")]
            IMAGE_UPLOAD_ERROR,
            [StringValue("moved users succefully")]
            MOVED_SUCCESS,
            [StringValue("nothing changed users")]
            NOTHING_CHANGE,
            [StringValue("error users succefully")]
            MOVED_ERROR,
            [StringValue("Wrong Id")]
            WRONG_ID,
            [StringValue("Wrong posted model")]
            WRONG_MODEL_VALIDATION

        }

        public string Status { get; set; }

        public string Message { get; set; }

        public StatusCode ReturnSuccesMessage(StatusCodeMessages message)
        {
            return new StatusCode() { Status = "Success", Message = StringEnum.GetStringValue(message) };
        }

        public StatusCode ReturnErrorMessage(StatusCodeMessages message)
        {
            return new StatusCode() { Status = "Error", Message = StringEnum.GetStringValue(message) };
        }
    }

}
