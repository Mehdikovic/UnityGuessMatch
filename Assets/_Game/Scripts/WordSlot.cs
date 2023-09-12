public class WordSlot : BaseSlot {
    private readonly string word;

    public WordSlot(string word) {
        this.word = word;
    }

    public string GetWord() => word;

    public override bool IsSameSlot(BaseSlot other) {
        return other is WordSlot otherSlot && word.StringEquals(otherSlot.word);
    }
}
