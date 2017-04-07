using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace diff.Tests
{
    [TestClass]
    public class DifferTests
    {
        IDiffer _differ = new Differ();
        
        [TestMethod]
        public void LeftNullAndRightNullTest()
        {
            byte[] left = null;
            byte[] right = null;

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.Equals, objectDiff.diffResultType);
            Assert.IsNull(objectDiff.diffs);
        }

        [TestMethod]
        public void LeftNullAndRightIsEmptyTest()
        {
            byte[] left = null;
            byte[] right = new byte[0];

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.Equals, objectDiff.diffResultType);
            Assert.IsNull(objectDiff.diffs);
        }

        [TestMethod]
        public void LeftNullAndRightIsNotEmptyTest()
        {
            byte[] left = null;
            byte[] right = {0, 0};

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.SizeDoNotMatch, objectDiff.diffResultType);
            Assert.IsNull(objectDiff.diffs);
        }

        [TestMethod]
        public void LeftIsEmptyAndRightIsNullTest()
        {
            byte[] left = new byte[0];
            byte[] right = null;

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.Equals, objectDiff.diffResultType);
            Assert.IsNull(objectDiff.diffs);
        }

        [TestMethod]
        public void LeftIsNotEmptyAndRightIsNullTest()
        {
            byte[] left = { 0, 0 };
            byte[] right = null;

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.SizeDoNotMatch, objectDiff.diffResultType);
            Assert.IsNull(objectDiff.diffs);
        }

        [TestMethod]
        public void LeftIs1ByteLongAndRightIs2ByteLongTest()
        {
            byte[] left = { 0, 0 };
            byte[] right = { 0 };

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.SizeDoNotMatch, objectDiff.diffResultType);
            Assert.IsNull(objectDiff.diffs);
        }


        [TestMethod]
        public void LeftAndRightAreEmptyTest()
        {
            byte[] left = new byte[0];
            byte[] right = new byte[0];

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.Equals, objectDiff.diffResultType);
            Assert.IsNull(objectDiff.diffs);
        }

        [TestMethod]
        public void LeftAndRightAre1ByteLongEqualsTest()
        {
            byte[] left = {0};
            byte[] right = {0};

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.Equals, objectDiff.diffResultType);
            Assert.IsNull(objectDiff.diffs);
        }

        [TestMethod]
        public void LeftAndRightAre2ByteLongEqualsTest()
        {
            byte[] left = { 0, 0 };
            byte[] right = { 0, 0 };

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.Equals, objectDiff.diffResultType);
            Assert.IsNull(objectDiff.diffs);
        }

        [TestMethod]
        public void LeftAndRightAreDifferentIn1ByteTest()
        {
            byte[] left = { 1, 0 };
            byte[] right = { 0, 0 };

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.ContentDoNotMatch, objectDiff.diffResultType);
            Assert.IsNotNull(objectDiff.diffs);
            Assert.AreEqual(objectDiff.diffs.Count, 1);
            Assert.AreEqual(objectDiff.diffs[0].offset, 0);
            Assert.AreEqual(objectDiff.diffs[0].length, 1);
        }

        [TestMethod]
        public void LeftAndRightAreDifferentIn2ConsecutiveBytesTest()
        {
            byte[] left = { 1, 1, 0 };
            byte[] right = { 0, 0, 0 };

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.ContentDoNotMatch, objectDiff.diffResultType);
            Assert.IsNotNull(objectDiff.diffs);
            Assert.AreEqual(objectDiff.diffs.Count, 1);
            Assert.AreEqual(objectDiff.diffs[0].offset, 0);
            Assert.AreEqual(objectDiff.diffs[0].length, 2);
        }

        [TestMethod]
        public void LeftAndRightAreDifferentInEveryByteTest()
        {
            byte[] left = { 1, 1, 1, 1 };
            byte[] right = { 0, 0, 0, 0 };

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.ContentDoNotMatch, objectDiff.diffResultType);
            Assert.IsNotNull(objectDiff.diffs);
            Assert.AreEqual(objectDiff.diffs.Count, 1);
            Assert.AreEqual(objectDiff.diffs[0].offset, 0);
            Assert.AreEqual(objectDiff.diffs[0].length, 4);
        }

        [TestMethod]
        public void LeftAndRightAreDifferentInFirstAndLastByteTest()
        {
            byte[] left = { 1, 1, 1, 1 };
            byte[] right = { 0, 1, 1, 0 };

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.ContentDoNotMatch, objectDiff.diffResultType);
            Assert.IsNotNull(objectDiff.diffs);
            Assert.AreEqual(objectDiff.diffs.Count, 2);
            Assert.AreEqual(objectDiff.diffs[0].offset, 0);
            Assert.AreEqual(objectDiff.diffs[0].length, 1);
            Assert.AreEqual(objectDiff.diffs[1].offset, 3);
            Assert.AreEqual(objectDiff.diffs[1].length, 1);
        }

        [TestMethod]
        public void LeftAndRightAreDifferentAsInAssignmentExampleTest()
        {
            byte[] left = { 1, 1, 1, 1 };
            byte[] right = { 0, 1, 0, 0 };

            ObjectDiff objectDiff = _differ.GetDiff(left, right);
            Assert.IsNotNull(objectDiff);
            Assert.AreEqual(DiffResultType.ContentDoNotMatch, objectDiff.diffResultType);
            Assert.IsNotNull(objectDiff.diffs);
            Assert.AreEqual(objectDiff.diffs.Count, 2);
            Assert.AreEqual(objectDiff.diffs[0].offset, 0);
            Assert.AreEqual(objectDiff.diffs[0].length, 1);
            Assert.AreEqual(objectDiff.diffs[1].offset, 2);
            Assert.AreEqual(objectDiff.diffs[1].length, 2);
        }

    }
}