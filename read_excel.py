import json
try:
    import pandas as pd
except ImportError:
    import os
    os.system("pip3 install pandas openpyxl")
    import pandas as pd

df = pd.read_excel('/Users/truongvncj/Desktop/ThanhMy/Tailieu/Pre_trip checklist.xlsx')
# drop all nan rows
df = df.dropna(how='all')
# Output as JSON
print(df.to_json(orient='records', force_ascii=False))
