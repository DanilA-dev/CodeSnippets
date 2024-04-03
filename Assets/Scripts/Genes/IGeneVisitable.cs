namespace Upclimbing.Genes
{
    public interface IGeneVisitable
    {
        public void AcceptVisit(IGeneVisitor visitor);
    }
}