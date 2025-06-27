import argparse
from PIL import Image
from transformers import AutoProcessor, AutoModelForCausalLM

parser = argparse.ArgumentParser(description="Image to text")
parser.add_argument("file", help="Full file path")
args = parser.parse_args()

modelImg2Txt = "microsoft/git-base-coco"

image = Image.open(args.file)

processorImg2Txt = AutoProcessor.from_pretrained(
    modelImg2Txt, cache_dir="cache")
modelImg2Txt = AutoModelForCausalLM.from_pretrained(
    modelImg2Txt, cache_dir="cache")

pixel_values = processorImg2Txt(
    images=image, return_tensors="pt").pixel_values
generated_ids = modelImg2Txt.generate(
    pixel_values=pixel_values, max_length=50)
generated_caption = processorImg2Txt.batch_decode(
    generated_ids, skip_special_tokens=True)[0]
print(generated_caption)