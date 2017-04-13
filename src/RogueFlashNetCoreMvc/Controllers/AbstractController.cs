using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using RogueFlashNetCoreMvc.Model;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class AbstractController : Controller
    {
        protected AppDbContext DbContext    { get; } = null;
        protected ILogger Logger            { get; } = null;


        private static readonly JsonResult emptyJson = new JsonResult(new object());


        public AbstractController(AppDbContext dbContext)
            : this(dbContext, null)
        {
            //
        }

        public AbstractController(ILoggerFactory loggerFactory)
            : this(null, loggerFactory)
        {
            //
        }


        public AbstractController(
                AppDbContext dbContext,
                ILoggerFactory loggerFactory)
        {
            DbContext = dbContext;

            if (loggerFactory != null)
            {
                Logger = loggerFactory.CreateLogger(GetType().Name);
            }
        }


        protected JsonResult GetEmptyJson()
        {
            return emptyJson;
        }

        protected JsonResult GetEmptyJsonError()
        {
            Response.StatusCode = 500;
            return emptyJson;
        }


        public string GetParameter(string parameter)
        {
            StringValues values = StringValues.Empty;
            var found = Request.Query.TryGetValue(
                parameter,
                out values);
            if (!found)
            {
                found = Request.Form.TryGetValue(
                    parameter,
                    out values);
            }
            if (found)
            {
                return values.ToString();
            }
            else
            {
                return "";
            }
        }

        protected bool GetBooleanParameter(string parameter)
        {
            var value = false;
            bool.TryParse(
                GetParameter(parameter),
                out value);
            return value;
        }

        protected int[] GetIntArrayParameter(string parameter)
        {
            var value = GetParameter(parameter);
            var stringValues = value.Split(',');

            var numberOfElements = stringValues.Length;
            if (numberOfElements == 0)
            {
                return new int[0];
            }

            try
            {
                var result = new int[numberOfElements];
                for (int i = 0, l = numberOfElements; i < l; i++)
                {
                    result[i] = int.Parse(stringValues[i]);
                }
                return result;
            }
            catch
            {
                return new int[0];
            }
        }
    }
}
