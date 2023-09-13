public class CardWord : Card {
    private readonly string word;

    public CardWord(int id, string word) : base(id) {
        this.word = word;
    }

    public override object Clone() {
        return new CardWord(GetID(), word);
    }

    public string GetWord() => word;
}
