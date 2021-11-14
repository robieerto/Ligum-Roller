import matplotlib.pyplot as plt
from matplotlib.ticker import MaxNLocator
from pandas import read_csv
from os import path, makedirs
import sys

graph_path = "data/graph/"

data = read_csv(sys.argv[1])
# Plot
x = data['vzdialenost']
y = data['priemer']
plt.plot(x, y)
plt.xlabel('Vzdialenosť (mm)')
plt.ylabel('Priemer (mm)')
# formatter = FuncFormatter(form4)
# plt.gca().yaxis.set_major_formatter(FuncFormatter(formatter))
plt.gca().get_yaxis().get_major_formatter().set_useOffset(False)
plt.gca().xaxis.set_major_locator(MaxNLocator(min_n_ticks=15))
plt.gca().yaxis.set_major_locator(MaxNLocator(min_n_ticks=30))
plt.gca().set_xlim(0)
plt.gca().set_ylim(bottom=max(data['priemer'])*0.99, top=max(data['priemer'])*1.005)
plt.grid(linestyle=':', linewidth=1)
plt.gcf().set_size_inches(18.5, 10.5)
makedirs(graph_path, exist_ok=True)
plt.savefig(graph_path + '%s.png' % path.splitext(path.basename(sys.argv[1]))[0])
print("Graph created")
