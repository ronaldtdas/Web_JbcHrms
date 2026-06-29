import os, re, json

raw = os.environ['FIREBASE_CONFIG']
# Extract object from JS snippet: firebaseConfig = { ... };
match = re.search(r'\{.*\}', raw, re.DOTALL)
if not match:
    raise SystemExit('Could not parse Firebase config from secret')
js_obj = match.group(0)
# Quote unquoted JS keys to make valid JSON
json_str = re.sub(r'(\w+)\s*:', r'"\1":', js_obj)
obj = json.loads(json_str)
# Map JS camelCase -> C# PascalCase
key_map = {
    'apiKey': 'ApiKey',
    'authDomain': 'AuthDomain',
    'databaseURL': 'DatabaseURL',
    'projectId': 'ProjectId',
    'storageBucket': 'StorageBucket',
    'messagingSenderId': 'MessagingSenderId',
    'appId': 'AppId',
}
mapped = {key_map.get(k, k): v for k, v in obj.items()}
with open('wwwroot/appsettings.json', 'w') as f:
    json.dump({'FirebaseConfig': mapped}, f, indent=2)
print('appsettings.json written successfully')
