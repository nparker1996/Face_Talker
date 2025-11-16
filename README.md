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

### View Logs
Once a call session is complete, a record of the session is saves to `log.json` under your local persistent data path. Here are a few examples:
- Windows - `C:\Users\<user>\AppData\LocalLow\FaceTalker\Face_Talker\`
- Android - `/storage/emulated/<userid>/Android/data/Face_Talker/files/`
- iOS - `/var/mobile/Containers/Data/Application/<guid>/Documents/`
if you cannot find your, please reference the [persistentDataPath Documentation](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html)).

Logs contain several pieces of data from each session:
- `sessionStartTime` (long) - The time (in ticks) the session started
- `sessionEndTime` (long) - The time (in ticks) the session ended
- `userWordCount` (int) - number of words used by the user
- `chatWordCount` (int) - number of words used by ChatGPT (the assistant)
- `queryCount` (int) - number of queries that the use made to ChatGPT
- `chatMessages` - a log of the conversation between the user and ChatGPT

## Architecture and Design

### Chat Session

#### Prompt

#### Conversation

### Analytics

#### During a Session

#### Data

#### Export to JSON

### GUI

#### Menu Structure

#### Design

### Other

## Noteable Packages
- OpenAI Unity - https://github.com/srcnalt/OpenAI-Unity
- Speech to Text - https://github.com/yasirkula/UnitySpeechToText

