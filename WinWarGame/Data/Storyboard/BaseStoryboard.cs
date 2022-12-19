using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarCS.Data.Storyboard
{
    enum IntroStage
    {
        None,
        Castle,
        CastleLoop,
        Swamp,
        SwampLoop,
        SwampFortressEnter,
        CaveEnter,
        CaveLoop,
        CaveExit,
        BlizzardLogo
    }

    enum AudioStage
    {
        None,
        InTheAgeOfChaos,
        TheKingdomOfAzeroth,
        NoOneKnewWhere,
        OpenGate,
        WithAnIngenious,
        WelcomeToTheWorld
    }

    internal delegate void StageSwitched(IntroStage newStage);
    internal delegate void AudioStageSwitched(AudioStage newStage);
    internal delegate void ChangeMovieStatus(bool shouldPlay);

    internal class BaseStoryboard
    {
        protected internal IntroStage Stage { get; protected set; }
        protected internal AudioStage AudioStage { get; protected set; }

        internal event StageSwitched OnStageSwitched;
        internal event AudioStageSwitched OnAudioStageSwitched;
        internal event ChangeMovieStatus OnChangeMovieStatus;

        protected double elapsedTimeInStage;
        internal float CurrentAlpha;

        internal BaseStoryboard()
        {
            CurrentAlpha = 1.0f;
            Stage = IntroStage.None;
            AudioStage = AudioStage.None;
        }

        internal void ChangeAudioState(AudioStage newStage)
        {
            // None is always allowed, but otherwise only allow increasing audio stages
            bool shouldSetState = (newStage == AudioStage.None) || ((int)AudioStage < (int)newStage);
            if (!shouldSetState)
            {
                return;
            }

            AudioStage = newStage;
            OnAudioStageSwitched?.Invoke(AudioStage);
        }

        internal virtual void Update(GameTime gameTime)
        {
            elapsedTimeInStage += gameTime.ElapsedGameTime.TotalSeconds;
        }

        internal virtual string GetCurrentIntroText()
        {
            return string.Empty;
        }

        protected void EnterStage(IntroStage setStage)
        {
            Stage = setStage;
            elapsedTimeInStage = 0;

            switch (setStage)
            {
                case IntroStage.Castle:
                    CurrentAlpha = 0.0f;
                    break;
            }

            InvokeStageSwitched(setStage);
        }

        internal void NotifyMovieDidFinish()
        {
            switch (Stage)
            {
                case IntroStage.Castle:
                    EnterStage(IntroStage.CastleLoop);
                    break;

                case IntroStage.Swamp:
                    EnterStage(IntroStage.SwampLoop);
                    break;

                case IntroStage.SwampFortressEnter:
                    EnterStage(IntroStage.CaveEnter);
                    break;

                case IntroStage.CaveEnter:
                    EnterStage(IntroStage.CaveLoop);
                    break;

                case IntroStage.CaveExit:
                    EnterStage(IntroStage.BlizzardLogo);
                    break;
            }
        }

        protected void InvokeChangeMovieStatus(bool shouldPlay)
        {
            OnChangeMovieStatus?.Invoke(shouldPlay);
        }

        protected void InvokeStageSwitched(IntroStage setStage)
        {
            OnStageSwitched?.Invoke(setStage);
        }
    }
}
