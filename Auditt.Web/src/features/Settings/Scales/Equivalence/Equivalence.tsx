import { EquivalenceModel } from "./EquivalenceModel";

interface Props {
    equivalences: EquivalenceModel[];
}

export const Equivalence = ({ equivalences }: Props) => {
    return (
        <div className="space-y-4">
            <div className="pl-8 space-y-2 text-sm font-bold">
                {equivalences.map((eq) => (
                    <label key={eq.id} className="flex flex-col mb-2">
                        <span>{eq.name}</span>
                        <span className="text-red-500">Value: {eq.value}</span>
                    </label>
                ))}
            </div>
        </div>
    );
};