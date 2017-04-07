using System;
using System.Web.Http;

namespace diff
{
    /// <summary>
    /// Provides endpoints for diff api v1
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("v1/diff")]
    public class DiffController : ApiController
    {
        private IDiffObjectRepository _diffObjectRepository;
        private IDiffer _differ;

        public DiffController()
        {
            //configuring controller with repository and diff algorithm implementations
            _diffObjectRepository = Container.DiffObjectRepository;
            _differ = Container.Differ;
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            //MethodSummary: get object, compare and return diff

            DiffObject diffObject;
            //if Left or Right properties were not set return not found
            if (!_diffObjectRepository.TryGet(id, out diffObject) || diffObject.Left == null || diffObject.Right == null)
            {
                return NotFound();
            }
            
            ObjectDiff objectDiff = _differ.GetDiff(diffObject.Left, diffObject.Right);

            return Ok(objectDiff);
        }

        [HttpPut]
        [Route("{id}/left")]
        public IHttpActionResult UpdateLeft(int id, ObjectData objectData)
        {
            if (objectData.data == null)
            {
                return BadRequest();
            }

            byte[] left = null;

            try
            {
                left = Convert.FromBase64String(objectData.data);
            }
            catch (FormatException e)
            {
                System.Diagnostics.Trace.WriteLine(e);
                //System behavior for non Base64 string input(PUT) is not specified. return BadRequest
                return BadRequest(e.Message);
            }

            _diffObjectRepository.AddOrUpdate(new DiffObject {Id = id, Left = left});

            return Created<DiffObject>(Request.RequestUri, null);
        }

        [HttpPut]
        [Route("{id}/right")]
        public IHttpActionResult UpdateRight(int id, ObjectData objectData)
        {
            if (objectData.data == null)
            {
                return BadRequest();
            }

            byte[] right = null;

            try
            {
                right = Convert.FromBase64String(objectData.data);
            }
            catch (FormatException e)
            {
                System.Diagnostics.Trace.WriteLine(e);
                //System behavior for non Base64 string intput(PUT) is not specified. return BadRequest
                return BadRequest(e.Message);
            }

            _diffObjectRepository.AddOrUpdate(new DiffObject { Id = id, Right = right });

            return Created<DiffObject>(Request.RequestUri, null);
        }
    }

    /// <summary>
    /// Stub for diff api v2 to show that api versioning is supported
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("v2/diff")]
    public class DiffV2Controller : ApiController
    {
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            return NotFound();
        }
    }
}
