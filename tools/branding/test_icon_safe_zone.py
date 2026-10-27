"""Checks that the ThogaiPay launcher icon is opaque and centered."""
from pathlib import Path
from PIL import Image

ROOT = Path(__file__).resolve().parents[2]
ICON = ROOT / "branding" / "brands" / "thogaipay" / "assets" / "app_icon.png"

def test_icon_is_square_and_has_opaque_background():
    image = Image.open(ICON).convert("RGBA")
    assert image.width == image.height
    assert min(image.getchannel("A").getextrema()) == 255

def test_logo_has_safe_padding_and_is_centered():
    image = Image.open(ICON).convert("RGBA")
    pixels = image.load()
    points = []
    for y in range(image.height):
        for x in range(image.width):
            r, g, b, a = pixels[x, y]
            if a > 0 and not (r > 245 and g > 245 and b > 245):
                points.append((x, y))
    assert points
    left = min(p[0] for p in points); right = max(p[0] for p in points)
    top = min(p[1] for p in points); bottom = max(p[1] for p in points)
    assert left > image.width * 0.08 and top > image.height * 0.08
    assert right < image.width * 0.92 and bottom < image.height * 0.92
    cx = (left + right) / 2; cy = (top + bottom) / 2
    assert abs(cx - image.width / 2) < image.width * 0.04
    assert abs(cy - image.height / 2) < image.height * 0.04
