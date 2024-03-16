﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WaveTracker.Tracker;

namespace WaveTracker.UI {
    public class FramesPanel : Panel {
        Texture2D arrow;
        FrameButton[] frames;
        PatternEditor patternEditor;
        public SpriteButton bNewFrame, bDeleteFrame, bDuplicateFrame, bMoveLeft, bMoveRight;
        //public Button increasePattern, decreasePattern;

        public FramesPanel(int x, int y, int width, int height) {
            InitializePanel("Frames", x, y, width, height);
        }

        public void Initialize(Texture2D sprite, GraphicsDevice device, PatternEditor patternEditor) {
            this.patternEditor = patternEditor;
            bNewFrame = new SpriteButton(4, 10, 15, 15, sprite, 19, this);
            bNewFrame.SetTooltip("Insert Frame", "Insert a new frame after this one");
            bDeleteFrame = new SpriteButton(19, 10, 15, 15, sprite, 24, this);
            bDeleteFrame.SetTooltip("Delete Frame", "Delete this frame from the track");
            bDuplicateFrame = new SpriteButton(34, 10, 15, 15, sprite, 17, this);
            bDuplicateFrame.SetTooltip("Duplicate Frame", "Create a copy of this frame and insert it after");

            bMoveLeft = new SpriteButton(4, 25, 15, 15, sprite, 20, this);
            bMoveLeft.SetTooltip("Move Left", "Move this frame to be earlier in the song");
            bMoveRight = new SpriteButton(19, 25, 15, 15, sprite, 21, this);
            bMoveRight.SetTooltip("Move Right", "Move this frame to be later in the song");

            //increasePattern = new Button("+", 484, 12, this);
            //increasePattern.width = 18;
            //increasePattern.SetTooltip("Increase Pattern", "Increase this frame's pattern");
            //decreasePattern = new Button("-", 484, 26, this);
            //decreasePattern.width = 18;
            //increasePattern.SetTooltip("Decrease Pattern", "Decrease this frame's pattern");

            // create arrow texture
            arrow = new Texture2D(device, 7, 4);
            Color[] data = new Color[7 * 4];
            Color arrowColor = new Color(8, 124, 232);
            for (int y = 0; y < 4; y++) {
                for (int x = 0; x < 7; ++x) {
                    if (x >= y && x < 7 - y)
                        data[x + y * 7] = arrowColor;
                    else
                        data[x + y * 7] = Color.Transparent;
                }
            }
            arrow.SetData(data);
            frames = new FrameButton[25];
            for (int i = 0; i < frames.Length; ++i) {
                frames[i] = new FrameButton(i - frames.Length / 2, patternEditor, this);
                frames[i].x = 54 + i * 18;
                frames[i].y = 21;
            }
        }

        public void Update() {
            foreach (FrameButton button in frames) {
                button.Update();
            }
            bDeleteFrame.enabled = App.CurrentSong.FrameSequence.Count > 1;
            bNewFrame.enabled = bDuplicateFrame.enabled = App.CurrentSong.FrameSequence.Count < 100;
            bMoveRight.enabled = patternEditor.cursorPosition.Frame < App.CurrentSong.FrameSequence.Count - 1;
            bMoveLeft.enabled = patternEditor.cursorPosition.Frame > 0;
            if (new Rectangle(80, 12, 397, 28).Contains(MouseX, MouseY) && Input.focus == null) {
                if (!Input.GetClick(KeyModifier._Any)) {
                    if (Input.MouseScrollWheel(KeyModifier.None) < 0) {
                        if (Playback.isPlaying && FrameEditor.followMode)
                            Playback.NextFrame();
                        else
                            patternEditor.NextFrame();
                    }
                    if (Input.MouseScrollWheel(KeyModifier.None) > 0) {
                        if (Playback.isPlaying && FrameEditor.followMode)
                            Playback.PreviousFrame();
                        else
                            patternEditor.PreviousFrame();
                    }
                }
            }
            if (!Playback.isPlaying) {
                if (bNewFrame.Clicked) {
                    patternEditor.InsertNewFrame();
                    //FrameEditor.thisSong.frames.Insert(++FrameEditor.currentFrame, new Frame());
                    //FrameEditor.Goto(FrameEditor.currentFrame, FrameEditor.currentRow);
                }
                if (bDuplicateFrame.Clicked || Input.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.D, KeyModifier.Ctrl)) {
                    patternEditor.DuplicateFrame();
                    //FrameEditor.thisSong.frames.Insert(FrameEditor.currentFrame + 1, FrameEditor.thisFrame.Clone());
                    //FrameEditor.Goto(FrameEditor.currentFrame, FrameEditor.currentRow);

                }
                if (bDeleteFrame.Clicked) {
                    patternEditor.RemoveFrame();
                    //FrameEditor.thisSong.frames.RemoveAt(FrameEditor.currentFrame);
                    //FrameEditor.currentFrame--;
                    //if (FrameEditor.currentFrame < 0)
                    //    FrameEditor.currentFrame = 0;
                    //FrameEditor.Goto(FrameEditor.currentFrame, FrameEditor.currentRow);
                }


                if (bMoveRight.Clicked) {
                    patternEditor.MoveFrameRight();
                    //FrameEditor.thisSong.frames.Reverse(FrameEditor.currentFrame++, 2);
                    //FrameEditor.Goto(FrameEditor.currentFrame, FrameEditor.currentRow);
                }
                if (bMoveLeft.Clicked) {
                    patternEditor.MoveFrameLeft();
                    //FrameEditor.thisSong.frames.Reverse(--FrameEditor.currentFrame, 2);
                    //FrameEditor.Goto(FrameEditor.currentFrame, FrameEditor.currentRow);
                }

                //if (increasePattern.Clicked) {
                //    patternEditor.IncreaseFramePatternIndex();
                //}
                //if (decreasePattern.Clicked) {
                //    patternEditor.DecreaseFramePatternIndex();
                //}
            }
        }

        public void Draw() {
            DrawPanel();
            DrawRect(1, 9, 52, 33, Color.White);
            DrawRect(0, 9, 1, 32, Color.White);
            bNewFrame.Draw();
            bDeleteFrame.Draw();
            bDuplicateFrame.Draw();
            bMoveLeft.Draw();
            bMoveRight.Draw();
            DrawRect(67, 11, 423, 29, new Color(32, 37, 64));
            //DrawSprite(arrow, 275, 15, new Rectangle(0, 0, 7, 4));
            foreach (FrameButton button in frames) {
                button.Draw();
            }
            DrawRect(54, 11, 13, 29, new Color(223, 224, 232));
            DrawRect(490, 11, 13, 29, new Color(223, 224, 232));

            //increasePattern.Draw();
            //decreasePattern.Draw();
        }
    }
}
