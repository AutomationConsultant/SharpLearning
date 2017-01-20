﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpLearning.Containers;
using SharpLearning.Containers.Matrices;
using SharpLearning.Metrics.Classification;
using SharpLearning.Neural.Layers;
using SharpLearning.Neural.Learners;
using SharpLearning.Neural.Loss;
using SharpLearning.Neural.Models;
using System;
using System.IO;
using System.Linq;

namespace SharpLearning.Neural.Test.Models
{
    [TestClass]
    public class ClassificationNeuralNetModelTest
    {
        [TestMethod]
        public void ClassificationNeuralNetModel_Predict_Single()
        {
            var numberOfObservations = 500;
            var numberOfFeatures = 5;
            var numberOfClasses = 5;

            var random = new Random(32);
            var observations = new F64Matrix(numberOfObservations, numberOfFeatures);
            observations.Initialize(() => random.NextDouble());
            var targets = Enumerable.Range(0, numberOfObservations).Select(i => (double)random.Next(0, numberOfClasses)).ToArray();

            var sut = ClassificationNeuralNetModel.Load(() => new StringReader(ClassificationNeuralNetModelText));

            var predictions = new double[numberOfObservations];
            for (int i = 0; i < numberOfObservations; i++)
            {
                predictions[i] = sut.Predict(observations.GetRow(i));
            }

            var evaluator = new TotalErrorClassificationMetric<double>();
            var actual = evaluator.Error(targets, predictions);

            Assert.AreEqual(0.77, actual);
        }

        [TestMethod]
        public void ClassificationNeuralNetModel_Predict_Multiple()
        {
            var numberOfObservations = 500;
            var numberOfFeatures = 5;
            var numberOfClasses = 5;

            var random = new Random(32);
            var observations = new F64Matrix(numberOfObservations, numberOfFeatures);
            observations.Initialize(() => random.NextDouble());
            var targets = Enumerable.Range(0, numberOfObservations).Select(i => (double)random.Next(0, numberOfClasses)).ToArray();

            var sut = ClassificationNeuralNetModel.Load(() => new StringReader(ClassificationNeuralNetModelText));

            var predictions = sut.Predict(observations);

            var evaluator = new TotalErrorClassificationMetric<double>();
            var actual = evaluator.Error(targets, predictions);

            Assert.AreEqual(0.77, actual);
        }

        [TestMethod]
        public void ClassificationNeuralNetModel_PredictProbability_Single()
        {
            var numberOfObservations = 500;
            var numberOfFeatures = 5;
            var numberOfClasses = 5;

            var random = new Random(32);
            var observations = new F64Matrix(numberOfObservations, numberOfFeatures);
            observations.Initialize(() => random.NextDouble());
            var targets = Enumerable.Range(0, numberOfObservations).Select(i => (double)random.Next(0, numberOfClasses)).ToArray();

            var sut = ClassificationNeuralNetModel.Load(() => new StringReader(ClassificationNeuralNetModelText));

            var predictions = new ProbabilityPrediction[numberOfObservations];
            for (int i = 0; i < numberOfObservations; i++)
            {
                predictions[i] = sut.PredictProbability(observations.GetRow(i));
            }

            var evaluator = new TotalErrorClassificationMetric<double>();
            var actual = evaluator.Error(targets, predictions.Select(p => p.Prediction).ToArray());

            Assert.AreEqual(0.77, actual);
        }

        [TestMethod]
        public void ClassificationNeuralNetModel_PredictProbability_Multiple()
        {
            var numberOfObservations = 500;
            var numberOfFeatures = 5;
            var numberOfClasses = 5;

            var random = new Random(32);
            var observations = new F64Matrix(numberOfObservations, numberOfFeatures);
            observations.Initialize(() => random.NextDouble());
            var targets = Enumerable.Range(0, numberOfObservations).Select(i => (double)random.Next(0, numberOfClasses)).ToArray();

            var sut = ClassificationNeuralNetModel.Load(() => new StringReader(ClassificationNeuralNetModelText));

            var predictions = sut.PredictProbability(observations);

            var evaluator = new TotalErrorClassificationMetric<double>();
            var actual = evaluator.Error(targets, predictions.Select(p => p.Prediction).ToArray());

            Assert.AreEqual(0.77, actual);
        }

        [TestMethod]
        public void ClassificationNeuralNetModel_Save()
        {
            var numberOfObservations = 500;
            var numberOfFeatures = 5;
            var numberOfClasses = 5;

            var random = new Random(32);
            var observations = new F64Matrix(numberOfObservations, numberOfFeatures);
            observations.Initialize(() => random.NextDouble());
            var targets = Enumerable.Range(0, numberOfObservations).Select(i => (double)random.Next(0, numberOfClasses)).ToArray();

            var net = new NeuralNet();
            net.Add(new InputLayer(numberOfFeatures));
            net.Add(new DenseLayer(10));
            net.Add(new SvmLayer(numberOfClasses));

            var learner = new ClassificationNeuralNetLearner(net, new AccuracyLoss());
            var sut = learner.Learn(observations, targets);

            var writer = new StringWriter();
            sut.Save(() => writer);

            var actual = writer.ToString();
            Assert.AreEqual(ClassificationNeuralNetModelText, actual);
        }

        string ClassificationNeuralNetModelText = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ClassificationNeuralNetModel xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" z:Id=\"1\" xmlns:z=\"http://schemas.microsoft.com/2003/10/Serialization/\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Models\">\r\n  <m_neuralNet xmlns:d2p1=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural\" z:Id=\"2\">\r\n    <d2p1:Layers xmlns:d3p1=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\" z:Id=\"3\" z:Size=\"5\">\r\n      <d3p1:anyType z:Id=\"4\" xmlns:d4p1=\"SharpLearning.Neural.Layers\" i:type=\"d4p1:InputLayer\">\r\n        <_x003C_ActivationFunc_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">Undefined</_x003C_ActivationFunc_x003E_k__BackingField>\r\n        <_x003C_Depth_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">5</_x003C_Depth_x003E_k__BackingField>\r\n        <_x003C_Height_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Height_x003E_k__BackingField>\r\n        <_x003C_Width_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Width_x003E_k__BackingField>\r\n      </d3p1:anyType>\r\n      <d3p1:anyType z:Id=\"5\" xmlns:d4p1=\"SharpLearning.Neural.Layers\" i:type=\"d4p1:DenseLayer\">\r\n        <Bias xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"6\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseVector\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_Count_x003E_k__BackingField>10</d5p1:_x003C_Count_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"7\" i:type=\"d6p1:DenseVectorStorageOffloat\">\r\n            <d6p1:Length>10</d6p1:Length>\r\n            <d6p1:Data z:Id=\"8\" z:Size=\"10\">\r\n              <d3p1:float>0.09036873</d3p1:float>\r\n              <d3p1:float>-0.0892849</d3p1:float>\r\n              <d3p1:float>0.0444062427</d3p1:float>\r\n              <d3p1:float>-0.130815387</d3p1:float>\r\n              <d3p1:float>-0.00408257265</d3p1:float>\r\n              <d3p1:float>0.0873014852</d3p1:float>\r\n              <d3p1:float>0.05351502</d3p1:float>\r\n              <d3p1:float>-0.008352563</d3p1:float>\r\n              <d3p1:float>0.06452984</d3p1:float>\r\n              <d3p1:float>0.0433647372</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_length xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">10</_length>\r\n          <_values z:Ref=\"8\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </Bias>\r\n        <BiasGradients xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <OutputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"9\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>10</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>1</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"10\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>1</d6p1:RowCount>\r\n            <d6p1:ColumnCount>10</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"11\" z:Size=\"10\">\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">10</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_rowCount>\r\n          <_values z:Ref=\"11\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </OutputActivations>\r\n        <Weights xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"12\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>10</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>5</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"13\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>5</d6p1:RowCount>\r\n            <d6p1:ColumnCount>10</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"14\" z:Size=\"50\">\r\n              <d3p1:float>0.200813591</d3p1:float>\r\n              <d3p1:float>0.379688352</d3p1:float>\r\n              <d3p1:float>0.09347456</d3p1:float>\r\n              <d3p1:float>0.05192381</d3p1:float>\r\n              <d3p1:float>0.48716864</d3p1:float>\r\n              <d3p1:float>-0.1788064</d3p1:float>\r\n              <d3p1:float>0.0610006638</d3p1:float>\r\n              <d3p1:float>-0.5019685</d3p1:float>\r\n              <d3p1:float>0.3057493</d3p1:float>\r\n              <d3p1:float>-0.227275446</d3p1:float>\r\n              <d3p1:float>0.04821579</d3p1:float>\r\n              <d3p1:float>-0.42584008</d3p1:float>\r\n              <d3p1:float>0.384420365</d3p1:float>\r\n              <d3p1:float>0.08392842</d3p1:float>\r\n              <d3p1:float>0.124209873</d3p1:float>\r\n              <d3p1:float>-0.186943412</d3p1:float>\r\n              <d3p1:float>0.0352960862</d3p1:float>\r\n              <d3p1:float>-0.405029356</d3p1:float>\r\n              <d3p1:float>0.173918709</d3p1:float>\r\n              <d3p1:float>0.0418237075</d3p1:float>\r\n              <d3p1:float>0.18925105</d3p1:float>\r\n              <d3p1:float>-0.178382039</d3p1:float>\r\n              <d3p1:float>0.365650117</d3p1:float>\r\n              <d3p1:float>-0.0237353258</d3p1:float>\r\n              <d3p1:float>-0.106557667</d3p1:float>\r\n              <d3p1:float>-0.0604510643</d3p1:float>\r\n              <d3p1:float>0.1531038</d3p1:float>\r\n              <d3p1:float>0.238406077</d3p1:float>\r\n              <d3p1:float>0.03968021</d3p1:float>\r\n              <d3p1:float>-0.338847876</d3p1:float>\r\n              <d3p1:float>-0.161837563</d3p1:float>\r\n              <d3p1:float>0.211115554</d3p1:float>\r\n              <d3p1:float>0.217933</d3p1:float>\r\n              <d3p1:float>0.325960547</d3p1:float>\r\n              <d3p1:float>0.3550528</d3p1:float>\r\n              <d3p1:float>0.20725818</d3p1:float>\r\n              <d3p1:float>-0.280959666</d3p1:float>\r\n              <d3p1:float>-0.251424879</d3p1:float>\r\n              <d3p1:float>-0.229880422</d3p1:float>\r\n              <d3p1:float>-0.08193218</d3p1:float>\r\n              <d3p1:float>0.5054163</d3p1:float>\r\n              <d3p1:float>-0.0879670456</d3p1:float>\r\n              <d3p1:float>-0.449818939</d3p1:float>\r\n              <d3p1:float>-0.08015076</d3p1:float>\r\n              <d3p1:float>-0.185535625</d3p1:float>\r\n              <d3p1:float>-0.151163638</d3p1:float>\r\n              <d3p1:float>-0.4016353</d3p1:float>\r\n              <d3p1:float>0.5736703</d3p1:float>\r\n              <d3p1:float>0.129372135</d3p1:float>\r\n              <d3p1:float>0.008544407</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">10</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">5</_rowCount>\r\n          <_values z:Ref=\"14\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </Weights>\r\n        <WeightsGradients xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <_x003C_ActivationFunc_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">Relu</_x003C_ActivationFunc_x003E_k__BackingField>\r\n        <_x003C_Depth_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">10</_x003C_Depth_x003E_k__BackingField>\r\n        <_x003C_Height_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Height_x003E_k__BackingField>\r\n        <_x003C_UseBatchNormalization_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">false</_x003C_UseBatchNormalization_x003E_k__BackingField>\r\n        <_x003C_Width_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Width_x003E_k__BackingField>\r\n        <m_delta xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <m_inputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n      </d3p1:anyType>\r\n      <d3p1:anyType z:Id=\"15\" xmlns:d4p1=\"SharpLearning.Neural.Layers\" i:type=\"d4p1:ActivationLayer\">\r\n        <ActivationDerivative xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"16\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>10</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>1</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"17\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>1</d6p1:RowCount>\r\n            <d6p1:ColumnCount>10</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"18\" z:Size=\"10\">\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">10</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_rowCount>\r\n          <_values z:Ref=\"18\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </ActivationDerivative>\r\n        <OutputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"19\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>10</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>1</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"20\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>1</d6p1:RowCount>\r\n            <d6p1:ColumnCount>10</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"21\" z:Size=\"10\">\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">10</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_rowCount>\r\n          <_values z:Ref=\"21\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </OutputActivations>\r\n        <_x003C_ActivationFunc_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">Relu</_x003C_ActivationFunc_x003E_k__BackingField>\r\n        <_x003C_Depth_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">10</_x003C_Depth_x003E_k__BackingField>\r\n        <_x003C_Height_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Height_x003E_k__BackingField>\r\n        <_x003C_Width_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Width_x003E_k__BackingField>\r\n        <m_activation z:Id=\"22\" xmlns:d5p1=\"SharpLearning.Neural.Activations\" i:type=\"d5p1:ReluActivation\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <m_delta xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <m_inputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n      </d3p1:anyType>\r\n      <d3p1:anyType z:Id=\"23\" xmlns:d4p1=\"SharpLearning.Neural.Layers\" i:type=\"d4p1:DenseLayer\">\r\n        <Bias xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"24\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseVector\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_Count_x003E_k__BackingField>5</d5p1:_x003C_Count_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"25\" i:type=\"d6p1:DenseVectorStorageOffloat\">\r\n            <d6p1:Length>5</d6p1:Length>\r\n            <d6p1:Data z:Id=\"26\" z:Size=\"5\">\r\n              <d3p1:float>0.02558287</d3p1:float>\r\n              <d3p1:float>0.08612125</d3p1:float>\r\n              <d3p1:float>-0.027990602</d3p1:float>\r\n              <d3p1:float>-0.0873520747</d3p1:float>\r\n              <d3p1:float>-0.0178734828</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_length xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">5</_length>\r\n          <_values z:Ref=\"26\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </Bias>\r\n        <BiasGradients xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <OutputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"27\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>5</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>1</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"28\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>1</d6p1:RowCount>\r\n            <d6p1:ColumnCount>5</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"29\" z:Size=\"5\">\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">5</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_rowCount>\r\n          <_values z:Ref=\"29\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </OutputActivations>\r\n        <Weights xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"30\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>5</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>10</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"31\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>10</d6p1:RowCount>\r\n            <d6p1:ColumnCount>5</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"32\" z:Size=\"50\">\r\n              <d3p1:float>0.235572025</d3p1:float>\r\n              <d3p1:float>0.409809977</d3p1:float>\r\n              <d3p1:float>0.124005288</d3p1:float>\r\n              <d3p1:float>0.4758867</d3p1:float>\r\n              <d3p1:float>-0.02261618</d3p1:float>\r\n              <d3p1:float>0.249528646</d3p1:float>\r\n              <d3p1:float>-0.5207995</d3p1:float>\r\n              <d3p1:float>-0.3472955</d3p1:float>\r\n              <d3p1:float>0.684357643</d3p1:float>\r\n              <d3p1:float>0.297215432</d3p1:float>\r\n              <d3p1:float>0.377030164</d3p1:float>\r\n              <d3p1:float>0.153914168</d3p1:float>\r\n              <d3p1:float>-0.508355856</d3p1:float>\r\n              <d3p1:float>-0.137209862</d3p1:float>\r\n              <d3p1:float>0.5675018</d3p1:float>\r\n              <d3p1:float>0.223381281</d3p1:float>\r\n              <d3p1:float>0.6407918</d3p1:float>\r\n              <d3p1:float>-0.0239588339</d3p1:float>\r\n              <d3p1:float>0.2422848</d3p1:float>\r\n              <d3p1:float>-0.307419658</d3p1:float>\r\n              <d3p1:float>-0.0925501958</d3p1:float>\r\n              <d3p1:float>0.0850930661</d3p1:float>\r\n              <d3p1:float>-0.307736546</d3p1:float>\r\n              <d3p1:float>0.539523542</d3p1:float>\r\n              <d3p1:float>0.2920651</d3p1:float>\r\n              <d3p1:float>-0.241250426</d3p1:float>\r\n              <d3p1:float>0.0328167826</d3p1:float>\r\n              <d3p1:float>0.334490657</d3p1:float>\r\n              <d3p1:float>0.08743553</d3p1:float>\r\n              <d3p1:float>0.110946119</d3p1:float>\r\n              <d3p1:float>0.159989864</d3p1:float>\r\n              <d3p1:float>0.3826271</d3p1:float>\r\n              <d3p1:float>-0.4838341</d3p1:float>\r\n              <d3p1:float>0.461475521</d3p1:float>\r\n              <d3p1:float>0.05249601</d3p1:float>\r\n              <d3p1:float>-0.3262866</d3p1:float>\r\n              <d3p1:float>-0.221990034</d3p1:float>\r\n              <d3p1:float>0.4320362</d3p1:float>\r\n              <d3p1:float>0.04270436</d3p1:float>\r\n              <d3p1:float>-0.006934167</d3p1:float>\r\n              <d3p1:float>-0.0509896576</d3p1:float>\r\n              <d3p1:float>0.442134947</d3p1:float>\r\n              <d3p1:float>-0.4114543</d3p1:float>\r\n              <d3p1:float>-0.115528293</d3p1:float>\r\n              <d3p1:float>-0.657616556</d3p1:float>\r\n              <d3p1:float>-0.250718832</d3p1:float>\r\n              <d3p1:float>0.2563246</d3p1:float>\r\n              <d3p1:float>-0.432885826</d3p1:float>\r\n              <d3p1:float>-0.7319321</d3p1:float>\r\n              <d3p1:float>-0.495734245</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">5</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">10</_rowCount>\r\n          <_values z:Ref=\"32\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </Weights>\r\n        <WeightsGradients xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <_x003C_ActivationFunc_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">Undefined</_x003C_ActivationFunc_x003E_k__BackingField>\r\n        <_x003C_Depth_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">5</_x003C_Depth_x003E_k__BackingField>\r\n        <_x003C_Height_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Height_x003E_k__BackingField>\r\n        <_x003C_UseBatchNormalization_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">false</_x003C_UseBatchNormalization_x003E_k__BackingField>\r\n        <_x003C_Width_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Width_x003E_k__BackingField>\r\n        <m_delta xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n        <m_inputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n      </d3p1:anyType>\r\n      <d3p1:anyType z:Id=\"33\" xmlns:d4p1=\"SharpLearning.Neural.Layers\" i:type=\"d4p1:SvmLayer\">\r\n        <NumberOfClasses xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">5</NumberOfClasses>\r\n        <OutputActivations xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" z:Id=\"34\" xmlns:d5p2=\"MathNet.Numerics.LinearAlgebra.Single\" i:type=\"d5p2:DenseMatrix\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">\r\n          <d5p1:_x003C_ColumnCount_x003E_k__BackingField>5</d5p1:_x003C_ColumnCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_RowCount_x003E_k__BackingField>1</d5p1:_x003C_RowCount_x003E_k__BackingField>\r\n          <d5p1:_x003C_Storage_x003E_k__BackingField xmlns:d6p1=\"urn:MathNet/Numerics/LinearAlgebra\" z:Id=\"35\" i:type=\"d6p1:DenseColumnMajorMatrixStorageOffloat\">\r\n            <d6p1:RowCount>1</d6p1:RowCount>\r\n            <d6p1:ColumnCount>5</d6p1:ColumnCount>\r\n            <d6p1:Data z:Id=\"36\" z:Size=\"5\">\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n              <d3p1:float>0</d3p1:float>\r\n            </d6p1:Data>\r\n          </d5p1:_x003C_Storage_x003E_k__BackingField>\r\n          <_columnCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">5</_columnCount>\r\n          <_rowCount xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\">1</_rowCount>\r\n          <_values z:Ref=\"36\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra.Single\" />\r\n        </OutputActivations>\r\n        <_x003C_ActivationFunc_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">Undefined</_x003C_ActivationFunc_x003E_k__BackingField>\r\n        <_x003C_Depth_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">5</_x003C_Depth_x003E_k__BackingField>\r\n        <_x003C_Height_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Height_x003E_k__BackingField>\r\n        <_x003C_Width_x003E_k__BackingField xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\">1</_x003C_Width_x003E_k__BackingField>\r\n        <m_delta xmlns:d5p1=\"http://schemas.datacontract.org/2004/07/MathNet.Numerics.LinearAlgebra\" i:nil=\"true\" xmlns=\"http://schemas.datacontract.org/2004/07/SharpLearning.Neural.Layers\" />\r\n      </d3p1:anyType>\r\n    </d2p1:Layers>\r\n  </m_neuralNet>\r\n  <m_targetNames xmlns:d2p1=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\" z:Id=\"37\" z:Size=\"5\">\r\n    <d2p1:double>0</d2p1:double>\r\n    <d2p1:double>1</d2p1:double>\r\n    <d2p1:double>2</d2p1:double>\r\n    <d2p1:double>3</d2p1:double>\r\n    <d2p1:double>4</d2p1:double>\r\n  </m_targetNames>\r\n</ClassificationNeuralNetModel>";
    }
}