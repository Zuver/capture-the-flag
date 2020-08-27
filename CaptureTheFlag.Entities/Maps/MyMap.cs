using CaptureTheFlag.Entities.Guns;
using CaptureTheFlag.Entities.Players;
using CaptureTheFlag.Entities.Teams;
using CaptureTheFlag.Entities.Walls;
using Engine.Drawing;
using Engine.Entities;
using Engine.Entities.Graphs;
using Engine.Physics.Bodies;
using Engine.Utilities;
using Microsoft.Xna.Framework;

namespace CaptureTheFlag.Entities.Maps
{
    public class MyMap : AbstractMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="size"></param>
        public MyMap(Vector2 size)
            : base(size)
        {
            Initialize();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        protected sealed override void Initialize()
        {
            // Circle wall in the middle of the map
            PrimitiveBuilder circleWallModel1 = new PrimitiveBuilder(Color.White);
            circleWallModel1.Add(PrimitiveFactory.Circle(Color.White, 100f, 24));
            CircleBody circleWallBody1 =
                (CircleBody)BodyFactory.Circle(true, 0f, 0f, 0f, true, 100f).SetPosition(Size / 2);
            AddWall(new CircleWall(circleWallBody1, circleWallModel1));

            // Blue-side circle wall in front of flag
            PrimitiveBuilder circleWallModel2 = new PrimitiveBuilder(Color.Blue);
            circleWallModel2.Add(PrimitiveFactory.Circle(Color.White, 75f, 24));
            CircleBody circleWallBody2 =
                (CircleBody)BodyFactory.Circle(true, 0f, 0f, 0f, true, 75f).SetPosition(Size * 0.25f);
            AddWall(new CircleWall(circleWallBody2, circleWallModel2));

            // Red-side circle wall in front of flag
            PrimitiveBuilder circleWallModel3 = new PrimitiveBuilder(Color.Red);
            circleWallModel3.Add(PrimitiveFactory.Circle(Color.White, 75f, 24));
            CircleBody circleWallBody3 =
                (CircleBody)BodyFactory.Circle(true, 0f, 0f, 0f, true, 75f).SetPosition(Size * 0.75f);
            AddWall(new CircleWall(circleWallBody3, circleWallModel3));

            // Blue-side circle wall in front of flag
            PrimitiveBuilder circleWallModel4 = new PrimitiveBuilder(Color.Blue);
            circleWallModel4.Add(PrimitiveFactory.Circle(Color.White, 75f, 24));
            CircleBody circleWallBody4 =
                (CircleBody)
                BodyFactory.Circle(true, 0f, 0f, 0f, true, 75f).SetPosition(new Vector2(Size.X * 0.25f, Size.Y * 0.75f));
            AddWall(new CircleWall(circleWallBody4, circleWallModel4));

            // Red-side circle wall in front of flag
            PrimitiveBuilder circleWallModel5 = new PrimitiveBuilder(Color.Blue);
            circleWallModel5.Add(PrimitiveFactory.Circle(Color.White, 75f, 24));
            CircleBody circleWallBody5 =
                (CircleBody)
                BodyFactory.Circle(true, 0f, 0f, 0f, true, 75f).SetPosition(new Vector2(Size.X * 0.75f, Size.Y * 0.25f));
            AddWall(new CircleWall(circleWallBody5, circleWallModel5));

            ConstructNodeGraphEdges();
            NodeGraph.Instance.BuildShortestPathData();

            ConstructOuterWalls(Size);

            // Build blue team
            CaptureTheFlagTeam blueTeam = new CaptureTheFlagTeam(
                "Blue team",
                new Vector2(25f, 25f),
                Color.CornflowerBlue);

            // Build red team
            CaptureTheFlagTeam redTeam = new CaptureTheFlagTeam(
                "Red team",
                new Vector2(AppSettingsFacade.WindowWidth - 25f, AppSettingsFacade.WindowHeight - 25f),
                Color.Crimson);

            // Add guns
            AddGun(new WeakGun(
                WeakGun.DefaultBody(Size * 0.4f),
                WeakGun.DefaultModel()));

            AddGun(new WeakGun(
                WeakGun.DefaultBody(Size * 0.6f),
                WeakGun.DefaultModel()));

            AddGun(new ShotGun(
                ShotGun.DefaultBody(new Vector2((float)AppSettingsFacade.WindowWidth / 2,
                    0.1f * AppSettingsFacade.WindowHeight)),
                ShotGun.DefaultModel()));

            AddGun(new ShotGun(
                ShotGun.DefaultBody(new Vector2((float)AppSettingsFacade.WindowWidth / 2,
                    0.9f * AppSettingsFacade.WindowHeight)),
                ShotGun.DefaultModel()));

            // Add spawn points
            blueTeam.AddSpawnPoint(blueTeam.GetBasePosition());
            blueTeam.AddSpawnPoint(blueTeam.GetBasePosition() + new Vector2(0f, 50f));

            // Add players
            UserPlayer blueUser = UserPlayer.Default(blueTeam, redTeam);
            BotPlayer blueBot1 = BotPlayer.Default(blueTeam, redTeam);

            blueUser.InitialSpawn();
            blueBot1.InitialSpawn();

            AddTeam(blueTeam);

            // Add spawn points
            redTeam.AddSpawnPoint(redTeam.GetBasePosition());
            redTeam.AddSpawnPoint(redTeam.GetBasePosition() + new Vector2(0f, -50f));

            // Add players
            BotPlayer redBot1 = BotPlayer.Default(redTeam, blueTeam);
            BotPlayer redBot2 = BotPlayer.Default(redTeam, blueTeam);

            redBot1.InitialSpawn();
            redBot2.InitialSpawn();

            AddTeam(redTeam);
        }

        protected override void ConstructOuterWalls(Vector2 size)
        {
            // Left wall
            Vector2 leftWallStartPosition = Vector2.Zero;
            Vector2 leftWallEndPosition = new Vector2(0f, size.Y);

            PrimitiveBuilder leftWallModel = new PrimitiveBuilder(Color.White);
            leftWallModel.Add(PrimitiveFactory.Line(Color.White, leftWallStartPosition, leftWallEndPosition));

            LineBody leftWallBody = BodyFactory.Line(true, 0f, 0f, 0f, true, leftWallStartPosition, leftWallEndPosition);

            Walls.Add(new LineWall(leftWallBody, leftWallModel));

            // Top wall
            Vector2 topWallStartPosition = Vector2.Zero;
            Vector2 topWallEndPosition = new Vector2(size.X, 0f);

            PrimitiveBuilder topWallModel = new PrimitiveBuilder(Color.White);
            topWallModel.Add(PrimitiveFactory.Line(Color.White, topWallStartPosition, topWallEndPosition));

            LineBody topWallBody = BodyFactory.Line(true, 0f, 0f, 0f, true, topWallStartPosition, topWallEndPosition);

            Walls.Add(new LineWall(topWallBody, topWallModel));

            // Right wall
            Vector2 rightWallStartPosition = new Vector2(size.X, 0f);
            Vector2 rightWallEndPosition = new Vector2(size.X, size.Y);

            PrimitiveBuilder rightWallModel = new PrimitiveBuilder(Color.White);
            rightWallModel.Add(PrimitiveFactory.Line(Color.White, rightWallStartPosition, rightWallEndPosition));

            LineBody rightWallBody = BodyFactory.Line(true, 0f, 0f, 0f, true, rightWallStartPosition,
                rightWallEndPosition);

            Walls.Add(new LineWall(rightWallBody, rightWallModel));

            // Bottom wall
            Vector2 bottomWallStartPosition = new Vector2(size.X, size.Y);
            Vector2 bottomWallEndPosition = new Vector2(0f, size.Y);

            PrimitiveBuilder bottomWallModel = new PrimitiveBuilder(Color.White);
            bottomWallModel.Add(PrimitiveFactory.Line(Color.White, bottomWallStartPosition, bottomWallEndPosition));

            LineBody bottomWallBody = BodyFactory.Line(true, 0f, 0f, 0f, true, bottomWallStartPosition,
                bottomWallEndPosition);

            Walls.Add(new LineWall(bottomWallBody, bottomWallModel));
        }
    }
}
