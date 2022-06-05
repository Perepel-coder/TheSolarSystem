
using OpenTK.Mathematics;

namespace TheSolarSystem
{
    public class CreatPlanet
    {
        public float rPlanet;
        public float distanceTrajectoryCenter;
        private int sectorCount;
        private int stackCount;

        private float actualRotationSpeed;
        public float rotationSpeed;
        private float actualPositionAngle;
        public float speedMovement;
        public Vector4 positionSphere;
        public Matrix4 rotationSphere;
        public Vector3 trajectoryCenter;

        public List<float> vertPlanet;
        public List<int> indexVertPlanet;

        public List<float> vertTrajectory;
        public List<int> indexVertTrajectory;

        public CreatPlanet(float r, int x, int y, float dtc, float rs, float sm, Vector4 pos, Vector3 tc)
        {
            rPlanet = r;
            sectorCount = x;
            stackCount = y;
            distanceTrajectoryCenter = dtc;
            rotationSpeed = rs;
            speedMovement = sm;
            positionSphere = pos;
            actualRotationSpeed = 0;
            actualPositionAngle = (float)Math.PI;
            trajectoryCenter = tc;

            vertPlanet = new List<float>();
            indexVertPlanet = new List<int>();
            vertTrajectory = new List<float>();
            indexVertTrajectory = new List<int>();
        }

        // Рисуем полигональную модель сферы, формируем нормали
        // и задаем коодинаты текстуры
        // Каждый полигон - это трапеция. Трапеции верхнего и
        // нижнего слоев вырождаются в треугольники


        private void CreatTrajectory()
        {
            for(float a = 0; a < 360; a += 1)
            {
                float alfa = (float)(a * Math.PI / 180.0f);
                var X = (float)(distanceTrajectoryCenter * Math.Cos(alfa));
                var Y = (float)(distanceTrajectoryCenter * Math.Sin(alfa));

                vertTrajectory.Add(X);
                vertTrajectory.Add(Y);
                vertTrajectory.Add(0);
            }
            for (int i = 0; i < vertTrajectory.Count; i++)
            {
                indexVertTrajectory.Add(i);
            }
        }

        private void CreatSphere()
        {
            float sectorStep = (float)(2 * Math.PI / sectorCount);
            float stackStep = (float)(Math.PI / stackCount);

            for (int i = 0; i <= stackCount; ++i)
            {
                float stackAngle = (float)(Math.PI / 2 - i * stackStep);
                float xy = (float)(rPlanet * Math.Cos(stackAngle));
                float z = (float)(rPlanet * Math.Sin(stackAngle));

                for (int j = 0; j <= sectorCount; ++j)
                {
                    float sectorAngle = j * sectorStep;

                    float x = (float)(xy * Math.Cos(sectorAngle));
                    float y = (float)(xy * Math.Sin(sectorAngle));
                    vertPlanet.Add(x);
                    vertPlanet.Add(y);
                    vertPlanet.Add(z);

                    float s = (float)j / sectorCount;
                    float t = (float)i / stackCount;
                    vertPlanet.Add(s);
                    vertPlanet.Add(t);
                }
            }

            #region
            //float sectorStep = (float)(2*Math.PI / sectorCount);
            //float stackStep = (float)(Math.PI / stackCount);

            //vertPlanet.Add(0);
            //vertPlanet.Add(0);
            //vertPlanet.Add(rPlanet);

            //vertPlanet.Add(0.5f);
            //vertPlanet.Add(0);

            //for (int i = 1; i < stackCount; i++)
            //{
            //    float sin = 0, cos = 0;
            //    sin = (float)Math.Sin((Math.PI / 2) - (i * stackStep));
            //    cos = (float)Math.Cos((Math.PI / 2) - (i * stackStep));

            //    float z = (float)(rPlanet * sin);

            //    float xy = (float)(rPlanet * cos);


            //    for (int j = 0; j < sectorCount; j++)
            //    {
            //        sin = (float)Math.Sin(j * sectorStep);
            //        cos = (float)Math.Cos(j * sectorStep);

            //        float x = (float)(xy * cos);
            //        float y = (float)(xy * sin);

            //        //  заполнить массив вершин
            //        vertPlanet.Add(x);
            //        vertPlanet.Add(y);
            //        vertPlanet.Add(z);

            //        vertPlanet.Add((float)j / sectorCount);
            //        vertPlanet.Add((float)i / stackCount);

            //    }
            //}

            //vertPlanet.Add(0);
            //vertPlanet.Add(0);
            //vertPlanet.Add(-rPlanet);

            //vertPlanet.Add(0.5f);
            //vertPlanet.Add(1);
            #endregion
        }

        private void GetIndexSphere()
        {
            for (int i = 0; i < stackCount; ++i)
            {
                int k1 = i * (sectorCount + 1);     // beginning of current stack
                int k2 = k1 + sectorCount + 1;      // beginning of next stack

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indexVertPlanet.Add(k1);
                        indexVertPlanet.Add(k2);
                        indexVertPlanet.Add(k1 + 1);
                    }

                    if (i != (stackCount - 1))
                    {
                        indexVertPlanet.Add(k1 + 1);
                        indexVertPlanet.Add(k2);
                        indexVertPlanet.Add(k2 + 1);
                    }
                }
            }

            #region
            //int k1 = 0, k2 = 0;
            //for (int i = 0; i < stackCount; i++)
            //{
            //    #region k1 - k2
            //    if (i == 0 || i ==1) 
            //    { 
            //        if(i == 0) { k1 = 0; k2 = 1; }
            //        if(i == 1) 
            //        { 
            //            k1 = 1; 
            //            k2 = k1 + sectorCount; 
            //        }
            //    }
            //    else
            //    {
            //        // точка на верхнем экваторе
            //        k1 = (i-1) * sectorCount + 1;
            //        // точка на нижнем экваторе
            //        k2 = k1 + sectorCount;
            //    }
            //    #endregion

            //    for (int j = 0; j < sectorCount; j++)
            //    {
            //        #region indexes
            //        if(i == 0)
            //        {
            //            if (j == sectorCount - 1)
            //            {
            //                indexVertPlanet.Add(k1 + 1);
            //                indexVertPlanet.Add(1);
            //                indexVertPlanet.Add(0);

            //                break;
            //            }
            //            indexVertPlanet.Add(k1 + 1);
            //            indexVertPlanet.Add(k1 + 2);
            //            indexVertPlanet.Add(0);
            //            k1++;
            //        }
            //        if (i != 0 && i != stackCount - 1)
            //        {
            //            if(j != sectorCount-1)
            //            {
            //                indexVertPlanet.Add(k1);
            //                indexVertPlanet.Add(k2);
            //                indexVertPlanet.Add(k1 + 1);

            //                indexVertPlanet.Add(k1 + 1);
            //                indexVertPlanet.Add(k2);
            //                indexVertPlanet.Add(k2 + 1);
            //                k1++; k2++;
            //            }
            //            else
            //            {
            //                indexVertPlanet.Add(k1);
            //                indexVertPlanet.Add(k2);
            //                indexVertPlanet.Add(k1 - sectorCount+1);

            //                indexVertPlanet.Add(k1 - sectorCount+1);
            //                indexVertPlanet.Add(k2);
            //                indexVertPlanet.Add(k1+1);
            //            }
            //        }
            //        if (i == stackCount - 1)
            //        {
            //            indexVertPlanet.Add(k1 + 1);
            //            indexVertPlanet.Add(k1);
            //            indexVertPlanet.Add(k2);
            //            k1++;
            //        }
            //        #endregion
            //    }
            //}
            #endregion
        }

        public void CreatNewPlanet()
        {
            CreatSphere();
            GetIndexSphere();
            CreatTrajectory();
        }

        public void CreatMovement(float rSpeed, float mSpeed)
        {
            if (actualRotationSpeed > 180) { actualRotationSpeed = 180 - actualRotationSpeed; }
            actualRotationSpeed += (rotationSpeed * rSpeed);
            rotationSphere = Mathematics.Rotation(actualRotationSpeed, 0, 0, 1);

            if (actualPositionAngle > 360) { actualPositionAngle = 360 - actualPositionAngle; }
            actualPositionAngle += (float)(speedMovement * mSpeed * Math.PI / 180.0f);
            positionSphere.X = (float)(distanceTrajectoryCenter * Math.Cos(actualPositionAngle) + trajectoryCenter.X);
            positionSphere.Y = (float)(distanceTrajectoryCenter * Math.Sin(actualPositionAngle) + trajectoryCenter.Y);
        }
    }
}
