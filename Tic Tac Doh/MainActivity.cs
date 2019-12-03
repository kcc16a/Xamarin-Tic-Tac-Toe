using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;


namespace Tic_Tac_Doh
{

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnClickListener
    {
        private readonly ImageButton[,] buttons = new ImageButton[3, 3];

        private bool player1Turn = true;

        private int roundCount = 0;

        private int player1Points = 0;
        private int player2Points = 0;

        private TextView textViewPlayer1;
        private TextView textViewPlayer2;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            textViewPlayer1 = FindViewById<TextView>(Resource.Id.text_view_p1);
            textViewPlayer2 = FindViewById<TextView>(Resource.Id.text_view_p2);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    string buttonId = "button_" + i.ToString() + j.ToString();
                    int buttonResId = Resources.GetIdentifier(buttonId, "id", PackageName);

                    buttons[i, j] = FindViewById<ImageButton>(buttonResId);
                    buttons[i, j].SetOnClickListener(this);
                    buttons[i, j].Tag = Resource.Drawable.resetBlock;
                }
            }

            Button buttonReset = FindViewById<Button>(Resource.Id.button_reset);
            buttonReset.SetOnClickListener(new ButtonResetOnClickListener(this));
            
        }

        class ButtonResetOnClickListener : Java.Lang.Object, View.IOnClickListener
        {
            readonly MainActivity m_mainActivity;
            public ButtonResetOnClickListener(MainActivity mainActivity)
            {
                m_mainActivity = mainActivity;
            }
            public void OnClick(View v)
            {
                m_mainActivity.ResetGame();
            }
        }


        public void OnClick(View v)
        {
            ImageButton view = v as ImageButton;
            if (!((int)view.Tag == Resource.Drawable.resetBlock))
            {
                return;
            }

            if (player1Turn)
            {
                view.SetImageResource(Resource.Drawable.aang0);
                view.Tag = Resource.Drawable.aang0;
            }
            else
            {
                view.SetImageResource(Resource.Drawable.korra0);
                view.Tag = Resource.Drawable.korra0;
            }

            roundCount++;

            if (CheckForWin())
            {
                if (player1Turn)
                {
                    Player1Wins();
                }
                else
                {
                    Player2Wins();
                }
            }
            else if (roundCount == 9)
            {
                Draw();
            }
            else
            {
                player1Turn = !player1Turn;
            }

        }

        private bool CheckForWin()
        {
            int[,] field = new int[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    field[i, j] = (int)buttons[i, j].Tag;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (field[i, 0] == (field[i, 1])
                        && field[i, 0] == (field[i, 2])
                        && !(field[i, 0] == Resource.Drawable.resetBlock))
                {
                    return true;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (field[0, i] == (field[1, i])
                        && field[0, i] == (field[2, i])
                        && !(field[0, i] == Resource.Drawable.resetBlock))
                {
                    return true;
                }
            }

            if (field[0, 0] == (field[1, 1])
                    && field[0, 0] == (field[2, 2])
                    && !(field[0, 0] == Resource.Drawable.resetBlock))
            {
                return true;
            }

            if (field[0, 2] == (field[1, 1])
                    && field[0, 2] == (field[2, 0])
                    && !(field[0, 2] == Resource.Drawable.resetBlock))
            {
                return true;
            }

            return false;
        }

        private void Player1Wins()
        {
            player1Points++;
            Toast.MakeText(this, "Player 1 wins!", ToastLength.Long).Show();
            UpdatePointsText();
            ResetBoard();
        }

        private void Player2Wins()
        {
            player2Points++;
            Toast.MakeText(this, "Player 2 wins!", ToastLength.Long).Show();
            UpdatePointsText();
            ResetBoard();
        }

        private void Draw()
        {
            Toast.MakeText(this, "Draw!", ToastLength.Long).Show();
            ResetBoard();
        }

        private void UpdatePointsText()
        {
            string text1 = "Player 1: " + player1Points.ToString();
            string text2 = "Player 2: " + player2Points.ToString();
            textViewPlayer1.SetText(text1.ToCharArray(), 0, text1.Length);
            textViewPlayer2.SetText(text2.ToCharArray(), 0, text2.Length);
        }

        private void ResetBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    buttons[i, j].SetImageResource(Resource.Drawable.resetBlock);
                    buttons[i, j].Tag = Resource.Drawable.resetBlock;
                }
            }

            roundCount = 0;
            player1Turn = true;
        }

        private void ResetGame()
        {
            player1Points = 0;
            player2Points = 0;
            UpdatePointsText();
            ResetBoard();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutInt("roundCount", roundCount);
            outState.PutInt("player1Points", player1Points);
            outState.PutInt("player2Points", player2Points);
            outState.PutBoolean("player1Turn", player1Turn);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);

            roundCount = savedInstanceState.GetInt("roundCount");
            player1Points = savedInstanceState.GetInt("player1Points");
            player2Points = savedInstanceState.GetInt("player2Points");

        }


    }
}