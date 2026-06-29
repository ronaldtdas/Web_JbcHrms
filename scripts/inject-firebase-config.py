import json
import os
import re
import sys
from pathlib import Path

raw = os.getenv("FIREBASE_CONFIG", "").strip()
if not raw:
    print("ERROR: FIREBASE_CONFIG is missing")
    sys.exit(1)

# Accept:
# firebaseConfig = { ... };
raw = re.sub(r'^\s*firebaseConfig\s*=\s*', '', raw, flags=re.IGNORECASE).strip()
raw = re.sub(r';\s*$', '', raw)

# Convert JS object-literal keys to JSON keys:
# { apiKey: "x", authDomain: "y" } -> { "apiKey": "x", "authDomain": "y" }
raw = re.sub(r'([{\s,])([A-Za-z_][A-Za-z0-9_]*)\s*:', r'\1"\2":', raw)

try:
    firebase_config = json.loads(raw)
except json.JSONDecodeError as e:
    print(f"ERROR: Invalid FIREBASE_CONFIG format at line {e.lineno}, col {e.colno}: {e.msg}")
    sys.exit(1)

# Example appsettings target update
appsettings_path = Path("wwwroot/appsettings.json")
if not appsettings_path.exists():
    appsettings_path = Path("appsettings.json")

if not appsettings_path.exists():
    print("ERROR: appsettings.json not found")
    sys.exit(1)

data = json.loads(appsettings_path.read_text(encoding="utf-8"))
data["Firebase"] = firebase_config
appsettings_path.write_text(json.dumps(data, indent=2), encoding="utf-8")

print("FIREBASE_CONFIG parsed and injected successfully")
