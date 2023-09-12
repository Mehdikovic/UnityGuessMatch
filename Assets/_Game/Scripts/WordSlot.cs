public class WordSlot : BaseSlot {
    private readonly string word;

    public WordSlot(int id, string word) : base(id) {
        this.word = word;
    }

    public override object Clone() {
        return new WordSlot(GetID(), word);
    }

    public string GetWord() => word;
}
