# Face Talker
A prototype assessment for Dreamface of an Android application that leverages speech-to-text (SST) and ChatGPT to roughly assess a user’s mental & cognitive state. 

## How To Use

### Setup Face Talker to Communicate with ChatGPT
You will need to setup an `auth.json` to hold your OpenAI `api_key` so that Face Talker can communicate with ChatGPT via OpenAI Unity package. The following is copied from the [OpenAI-Unity](https://github.com/srcnalt/OpenAI-Unity) README by [srcnalt](https://github.com/srcnalt):

#### Setting Up Your OpenAI Account
To use the OpenAI API, you need to have an OpenAI account. Follow these steps to create an account and generate an API key:

- Go to https://openai.com/api and sign up for an account
- Once you have created an account, go to https://beta.openai.com/account/api-keys
- Create a new secret key and save it

#### Saving Your Credentials
To make requests to the OpenAI API, you need to use your API key and organization name (if applicable). To avoid exposing your API key in your Unity project, you can save it in your device's local storage.

To do this, follow these steps:

- Create a folder called .openai in your home directory (e.g. `C:User\UserName\` for Windows or `~\` for Linux or Mac)
- Create a file called `auth.json` in the `.openai` folder
- Add an api_key field and a organization field (if applicable) to the `auth.json` file and save it
- Here is an example of what your auth.json file should look like:

```json
{
    "api_key": "sk-...W6yi",
    "organization": "org-...L7W"
}
```

### Navigating  Application
The main function of the application is to allow the user to talk with ChatGPT, this is done via the "Make a Call" button on the main menu.

In the Chat menu you can type or speak your queries (via the "start speaking" button). Once you are satisfied, click "Send Message", ChatGPT will quickly respond. Once you are done with the conversation, you can click "End Call" to end the session and return to the main menu.

In the Analytics menu you can see data from the call sessions that have occurred during the current application use.

### View Logs
Once a call session is complete, a record of the session is saves to `log.json` under your local persistent data path. Here are a few examples of the data path:
- Windows - `C:\Users\<user>\AppData\LocalLow\FaceTalker\Face_Talker\`
- Android - `/storage/emulated/<userid>/Android/data/Face_Talker/files/`
- iOS - `/var/mobile/Containers/Data/Application/<guid>/Documents/`

if you cannot find yours, please reference the [persistentDataPath Documentation](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html).

Logs contain several pieces of data from each session:
- `sessionStartTime` (long) - The time (in ticks) the session started
- `sessionEndTime` (long) - The time (in ticks) the session ended
- `userWordCount` (int) - number of words used by the user
- `chatWordCount` (int) - number of words used by ChatGPT (the assistant)
- `queryCount` (int) - number of queries that the user made to ChatGPT
- `chatMessages` - a log of the conversation between the user and ChatGPT

You can read more about the data and analytics in the corrasponding sections below.

## Architecture and Design

### Chat Session

#### Prompt & Conversation
> Act as a care taker sitting right next to the user. Assume the user is above the age of 50. You are not a profession mental health expert. Have a converstation to gauge their mental & cognitive state. Use a friendly, warm, supportive tone throughout. Always encourage self-compassion and a hopeful outlook. Let the user direct the conversation, but make sure user is thinking keeping the conversation related to their mental & cognitive state. Don't break character. Don't ever mention that you are an AI model. Avoid going over 100 words per response.

Breaking down the prompt, I wanted to make it feels like a caretaker but not a mental health expert; so that it was kind and caring but did not claim to be a professional. This application is targeting older users that might be lonely or do not have much mental stimulation, so keeping engagement with the user was a high priority. I wanted chatGPT to allow the user to direct the conversation but also try to direct it back to their mental state, so that the user does not feel pressured to discuss things they do not want to.

Overall the initial prompt has made chatGPT very kind and considerate to the user, not pushing them too hard but making sure the user stays engaged with the converstion. 

#### Speech-To-Text
Using the `ISpeechToTextListener` package, I implemented a pretty basic STT that is activated by clicking the "Start Speaking" Button. This inputs the speech into the input field, the same place a user would type. This is so there are no additional steps to submitting a query.

Note: if you are using the STT while in the Unity Editor, it will automatically do a 'Hello World' test message, this is do to how the [UnitySpeechToText](https://github.com/yasirkula/UnitySpeechToText) package is configured.

### Analytics
A `SessionData` object is created for each session when it starts. This object hold the analytic data from the session. And is exported to the `log.json` when the session ends.

#### Data
Going off the Assessment Document provided, it mentioned "word count, usage time, apps opened, etc.". From that I came up with the six data pieces that would be useful to fulfill those mentioned ones and others that I felt worked well when communicating with chatGPT.
- `sessionStartTime` & `sessionEndTime` - Can be used to determine "usage time" and is generally good data to have
- `userWordCount` & `chatWordCount` - Fulfills "word count" requirement, I seperated out the counts into 'user' and 'chat' to make it easier to see who is carrying the conversation
- `queryCount` - Felt it was good to know as Face Talker is querying chatGPT
- `chatMessages` - Having a history of the conversation is good for review and to check if someone is trying to use strange queries prompts

#### During a Session
 Most of the analytics data is only updated on the `SessionData` when an user ends the call. I did this so the session was only being updated at one moment and any additions would only be needed to be put in one spot. 
 The except is `queryCount` which is updated when an user sends a message to ChatGPT. This was the simpliest way to record when a player sent a query to chatGPT.

#### Export to JSON
Exporting to the `log.json` in the local persistent data path was the simplest path to generate a file and handled easily by Unity.

### GUI
The layout of the button and info on each page focused on making sure things were easily readable and clickable by a user on a phone. I specifically thought about how older users might need the bigger text so they could read it better.

The design of the application is pretty simple, only having the needed functionality requested for the prototype. I also did not want to over complicate selection if an older user was using it.

## Noteable Packages
- OpenAI Unity - https://github.com/srcnalt/OpenAI-Unity
- Speech to Text - https://github.com/yasirkula/UnitySpeechToText

